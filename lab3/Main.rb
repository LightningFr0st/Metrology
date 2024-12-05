require 'ripper'
require 'pp'
require 'json'
require 'set'

class RubyAnalyzer < Ripper::SexpBuilder
  def initialize(source)
    super(source)
    @identifier_spen = {}
    @read_vars = Set.new
    @written_vars = Set.new

    #@multi_written_vars = Set.new

    @control_vars = Set.new
    @unused_vars = Set.new
    @input_vars = Set.new
    @input_output_vars = Set.new
    @get_vars = Set.new

  end

  def analyze
    sexp = parse
    File.open("sexp.rb", "w") { |file| PP.pp(sexp, file) }

    traverse(sexp)

    classify_variables

    @identifier_spen.each do |name, data|
      data[:count] = [data[:count] - 1, 0].max
    end

    {
      total_spen: calculate_total_spen,
      identifier_spen: @identifier_spen,

      input_vars: @input_vars.to_a,
      control_vars: @control_vars.to_a,
      modifiable_vars: @modifiable_vars.to_a,
      unused_vars: @unused_vars.to_a,

      io_input_vars: (@input_vars & @input_output_vars).to_a,
      io_control_vars: (@control_vars & @input_output_vars).to_a,
      io_modifiable_vars: (@modifiable_vars & @input_output_vars).to_a,
      io_unused_vars: (@unused_vars & @input_output_vars).to_a,

      chipin_metric: calculate_chipin_metric,
      io_chipin_metric: calculate_io_chipin_metric
    }

  end

  private

  def traverse(node)
    return unless node.is_a?(Array)

    case node[0]
    when :var_ref
      name = node[1][1]
      @read_vars.add(name)
      @unused_vars.delete(name)
      add_to_spen(name)
    when :assign
      name = node[1][1][1]
      if input_from_gets?(node[2])
        @input_vars.add(name)
        @input_output_vars.add(name)
      else
        @written_vars.add(name)
      end

      traverse(node[2])
      add_to_spen(name)
    when :if, :while, :until, :case, :for
      extract_control_vars(node)
    when :command
      if node[1][1] == "puts"
        process_puts(node)
      end
    else
      node.each { |subnode| traverse(subnode) if subnode.is_a?(Array) }
    end
  end

  def process_puts(node)
    arguments = node[2]
    return unless arguments.is_a?(Array)

    arguments.each do |arg|
      traverse_puts_arguments(arg)
    end

  end

  def traverse_puts_arguments(arg)
    if arg.is_a?(Array)
      if arg[0] == :var_ref
        @input_output_vars.add(arg[1][1])
        @read_vars.add(arg[1][1])
        add_to_spen(arg[1][1])
      else
        # Рекурсивно обходим аргументы
        arg.each { |subarg| traverse_puts_arguments(subarg) if subarg.is_a?(Array) }
      end
    end
  end

  def input_from_gets?(node)
    return false unless node.is_a?(Array)

    if node[0] == :vcall && node[1][1] == 'gets'
      return true
    end

    node.any? { |subnode| input_from_gets?(subnode) if subnode.is_a?(Array) }

  end

  def handle_assignment(name)

    # if @written_once_vars.include?(name)
    #   # Переменная записана повторно
    #   @multi_written_vars.add(name)
    #   @written_once_vars.delete(name)
    # elsif !@multi_written_vars.include?(name)
    #   # Переменная записана впервые
    #   @written_once_vars.add(name)
    # end

  end

  def add_to_spen(name)

    @identifier_spen[name] ||= { count: 0 }
    @identifier_spen[name][:count] += 1
  end

  def extract_control_vars(node)
    return unless node.is_a?(Array) && node.any?

    # Извлекаем условие (обычно второй элемент)
    condition_node = node[1]
    # Убедимся, что условие — это массив и обработаем его
    if condition_node.is_a?(Array)
      condition_node.each do |subnode|
        if subnode.is_a?(Array) && subnode[0] == :var_ref
          # Добавляем переменную в управляющие
          @control_vars.add(subnode[1][1])
        else
          # Рекурсивно обрабатываем подузлы условия
          traverse(subnode)
        end
      end
    end

    # Продолжаем обход основного узла (например, тела if или case)
    node.each { |subnode| traverse(subnode) if subnode.is_a?(Array) }
  end

  def classify_variables
    # Неиспользованные переменные: записаны, но не читаются
    @unused_vars = (@written_vars - @read_vars) | (@input_vars - @read_vars);

    # Контрольные переменные: используются в конструкциях управления
    @control_vars &= @read_vars

    # Вводимые переменные (R):
    # Читаются, но не записываются и не являются управляющими
    @input_vars = (@input_vars & @read_vars) - @control_vars - @written_vars;

    # Модифицируемые переменные (M)
    @modifiable_vars = @written_vars - @control_vars - @unused_vars;
  end

  def calculate_chipin_metric
    {
      R: @input_vars.size,
      M: @modifiable_vars.size,
      C: @control_vars.size,
      T: @unused_vars.size
    }
  end

  def calculate_io_chipin_metric
    io_vars = @input_output_vars
    {
      R: (io_vars & @input_vars).size, # IO переменные из группы R
      M: (io_vars & @modifiable_vars).size, # IO переменные из группы M
      C: (io_vars & @control_vars).size, # IO переменные из группы C
      T: (io_vars & @unused_vars).size # IO переменные из группы T
    }
  end

  def calculate_total_spen
    @identifier_spen.values.sum { |data| data[:count] }
  end
end

if __FILE__ == $0

  if ARGV.length < 1
    exit 1
  end
  
  input_file_path = ARGV[0]
  output_file_path = "result.json"
  
  unless File.exist?(input_file_path)
    puts "Ошибка: файл не найден по пути '#{input_file_path}'"
    exit 1
  end
  
  source_code = File.read(input_file_path)


  analyzer = RubyAnalyzer.new(source_code)
  result = analyzer.analyze

  # Записываем результат в выходной файл
  File.write(output_file_path, JSON.generate(result))

end