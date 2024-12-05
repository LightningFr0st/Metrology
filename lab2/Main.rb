require 'ripper'
require 'json'
require 'pp'

class RubyAnalyzer < Ripper::SexpBuilder
  def initialize(source)
    super(source)
    @condition_count = 0
    @max_nesting_level = 0
    @operator_count = 0
    @current_nesting_level = 0
  end

  def analyze
    sexp = parse
    #pp sexp
    traverse(sexp)
    {
      condition_count: @condition_count,
      max_nesting_level: @max_nesting_level,
      operator_count: @operator_count,
      complexity_ratio: calculate_complexity_ratio
    }
  end

  private

  def traverse(node)

    return unless node.is_a?(Array)

    case node[0]
    when :if, :while, :until, :elsif, :unless, :when, :for
      @operator_count += 1
      @condition_count += 1
      @current_nesting_level += 1
      @max_nesting_level = [@max_nesting_level, @current_nesting_level].max

      node.each { |subnode| traverse(subnode) }

      @current_nesting_level -= 1

    when :case
      # Обрабатываем все ветки 'when'
      node.each { |subnode| traverse(subnode) }
    when :assign, :binary, :dot2, :dot3, :call, :top_const_field, :fcall, :opassign
      @operator_count += 1
      node.each { |subnode| traverse(subnode) }
    else
      node.each { |subnode| traverse(subnode) if subnode.is_a?(Array) }
    end
  end
  
  def calculate_complexity_ratio
    return 0 if @operator_count.zero?
    (@condition_count.to_f / (@operator_count)).round(2)
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