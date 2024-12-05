using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Jilb
{
    public partial class MainForm : Form
    {
        string? filePath = null;

        private double a1 = 1;
        private double a2 = 2;
        private double a3 = 3;
        private double a4 = 0.5;

        public MainForm()
        {
            InitializeComponent();

            tableLayoutPanel1.Controls.Add(new Label() { Text = "Group" }, 0, 0);
            tableLayoutPanel1.Controls.Add(new Label() { Text = "Variables" }, 0, 1);
            tableLayoutPanel1.Controls.Add(new Label() { Text = "Count" }, 0, 2);

            tableLayoutPanel1.Controls.Add(new Label() { Text = "P", TextAlign = ContentAlignment.MiddleCenter }, 1, 0);
            tableLayoutPanel1.Controls.Add(new Label() { Text = "M", TextAlign = ContentAlignment.MiddleCenter }, 2, 0);
            tableLayoutPanel1.Controls.Add(new Label() { Text = "C", TextAlign = ContentAlignment.MiddleCenter }, 3, 0);
            tableLayoutPanel1.Controls.Add(new Label() { Text = "T", TextAlign = ContentAlignment.MiddleCenter }, 4, 0);


            tableLayoutPanel2.Controls.Add(new Label() { Text = "Group" }, 0, 0);
            tableLayoutPanel2.Controls.Add(new Label() { Text = "Variables" }, 0, 1);
            tableLayoutPanel2.Controls.Add(new Label() { Text = "Count" }, 0, 2);

            tableLayoutPanel2.Controls.Add(new Label() { Text = "P", TextAlign = ContentAlignment.MiddleCenter }, 1, 0);
            tableLayoutPanel2.Controls.Add(new Label() { Text = "M", TextAlign = ContentAlignment.MiddleCenter }, 2, 0);
            tableLayoutPanel2.Controls.Add(new Label() { Text = "C", TextAlign = ContentAlignment.MiddleCenter }, 3, 0);
            tableLayoutPanel2.Controls.Add(new Label() { Text = "T", TextAlign = ContentAlignment.MiddleCenter }, 4, 0);
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Title = "Select a file";
            fDialog.InitialDirectory = @"D:\University\Labs\5\metra\SpenChipin\files";
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = fDialog.FileName;
                codeTextBox.Text = File.ReadAllText(filePath);
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            ProcessStartInfo info = new ProcessStartInfo(@"ruby");
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.ArgumentList.Add(@"Main.rb");
            info.ArgumentList.Add($"{filePath}");
            Process.Start(info);
            Thread.Sleep(1000);
            ParseJSON();
        }

        private void ParseJSON()
        {
            string rawData = File.ReadAllText("result.json");
            JsonDocument doc = JsonDocument.Parse(rawData);
            JsonElement root = doc.RootElement;

            string total_spen = root.GetProperty("total_spen").GetInt32().ToString();

            label11.Text = total_spen;


            textBox1.Text = "";
            JsonElement identifierSpen = root.GetProperty("identifier_spen");
            foreach (JsonProperty property in identifierSpen.EnumerateObject())
            {
                string variableName = property.Name;
                if (property.Value.TryGetProperty("count", out JsonElement countElement))
                {
                    string count = countElement.GetRawText();
                    textBox1.Text += variableName + " : " + count + Environment.NewLine;
                }
            }

            //CHIPIN

            string res = "";
            var vars = root.GetProperty("input_vars").EnumerateArray();
            res = string.Join(" ", vars.Select(element => element.GetString()));
            tableLayoutPanel1.Controls.Add(new System.Windows.Forms.TextBox()
            {
                Dock = DockStyle.Fill,
                Text = res,
                AutoSize = false,
                Multiline = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical
            }, 1, 1);


            res = "";
            vars = root.GetProperty("modifiable_vars").EnumerateArray();
            res = string.Join(" ", vars.Select(element => element.GetString()));
            tableLayoutPanel1.Controls.Add(new System.Windows.Forms.TextBox()
            {
                Dock = DockStyle.Fill,
                Text = res,
                AutoSize = false,
                Multiline = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical
            }, 2, 1);

            res = "";
            vars = root.GetProperty("control_vars").EnumerateArray();
            res = string.Join(" ", vars.Select(element => element.GetString()));
            tableLayoutPanel1.Controls.Add(new System.Windows.Forms.TextBox()
            {
                Dock = DockStyle.Fill,
                Text = res,
                AutoSize = false,
                Multiline = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical
            }, 3, 1);

            res = "";
            vars = root.GetProperty("unused_vars").EnumerateArray();
            res = string.Join(" ", vars.Select(element => element.GetString()));
            tableLayoutPanel1.Controls.Add(new System.Windows.Forms.TextBox()
            {
                Dock = DockStyle.Fill,
                Text = res,
                AutoSize = false,
                Multiline = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical
            }, 4, 1);

            JsonElement chipin_metric = root.GetProperty("chipin_metric");
            {
                int P = chipin_metric.GetProperty("R").GetInt32();
                int M = chipin_metric.GetProperty("M").GetInt32();
                int C = chipin_metric.GetProperty("C").GetInt32();
                int T = chipin_metric.GetProperty("T").GetInt32();

                double calculation = a1 * P + a2 * M + a3 * C + a4 * T;

                label5.Text = calculation.ToString();

                tableLayoutPanel1.Controls.Add(new Label() { Text = P.ToString() }, 1, 2);
                tableLayoutPanel1.Controls.Add(new Label() { Text = M.ToString() }, 2, 2);
                tableLayoutPanel1.Controls.Add(new Label() { Text = C.ToString() }, 3, 2);
                tableLayoutPanel1.Controls.Add(new Label() { Text = T.ToString() }, 4, 2);
            }


            // IO CHIPIN

            res = "";
            vars = root.GetProperty("io_input_vars").EnumerateArray();
            res = string.Join(" ", vars.Select(element => element.GetString()));
            tableLayoutPanel2.Controls.Add(new System.Windows.Forms.TextBox()
            {
                Dock = DockStyle.Fill,
                Text = res,
                AutoSize = false,
                Multiline = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical
            }, 1, 1);


            res = "";
            vars = root.GetProperty("io_modifiable_vars").EnumerateArray();
            res = string.Join(" ", vars.Select(element => element.GetString()));
            tableLayoutPanel2.Controls.Add(new System.Windows.Forms.TextBox()
            {
                Dock = DockStyle.Fill,
                Text = res,
                AutoSize = false,
                Multiline = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical
            }, 2, 1);

            res = "";
            vars = root.GetProperty("io_control_vars").EnumerateArray();
            res = string.Join(" ", vars.Select(element => element.GetString()));
            tableLayoutPanel2.Controls.Add(new System.Windows.Forms.TextBox()
            {
                Dock = DockStyle.Fill,
                Text = res,
                AutoSize = false,
                Multiline = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical
            }, 3, 1);

            res = "";
            vars = root.GetProperty("io_unused_vars").EnumerateArray();
            res = string.Join(" ", vars.Select(element => element.GetString()));
            tableLayoutPanel2.Controls.Add(new System.Windows.Forms.TextBox()
            {
                Dock = DockStyle.Fill,
                Text = res,
                AutoSize = false,
                Multiline = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical
            }, 4, 1);

            chipin_metric = root.GetProperty("io_chipin_metric");
            {
                int P = chipin_metric.GetProperty("R").GetInt32();
                int M = chipin_metric.GetProperty("M").GetInt32();
                int C = chipin_metric.GetProperty("C").GetInt32();
                int T = chipin_metric.GetProperty("T").GetInt32();

                double calculation = a1 * P + a2 * M + a3 * C + a4 * T;

                label6.Text = calculation.ToString();

                tableLayoutPanel2.Controls.Add(new Label() { Text = P.ToString() }, 1, 2);
                tableLayoutPanel2.Controls.Add(new Label() { Text = M.ToString() }, 2, 2);
                tableLayoutPanel2.Controls.Add(new Label() { Text = C.ToString() }, 3, 2);
                tableLayoutPanel2.Controls.Add(new Label() { Text = T.ToString() }, 4, 2);
            }

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
