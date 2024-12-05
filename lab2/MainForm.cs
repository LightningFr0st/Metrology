using System.Diagnostics;
using System.Text.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Jilb
{
    public partial class MainForm : Form
    {
        string? filePath = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Title = "Select a file";
            fDialog.InitialDirectory = @"D:\University\5\lab\Метрология_2024\Jilb\files";
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

            string? conditionCount = root.GetProperty("condition_count").GetInt32().ToString();
            if (conditionCount != null)
            {
                label11.Text = conditionCount;
            }
            string? maxNestingLevel = (root.GetProperty("max_nesting_level").GetInt32() - 1).ToString();
            if (maxNestingLevel != null)
            {
                label12.Text = maxNestingLevel;
            }
            string? operatorCount = root.GetProperty("operator_count").GetInt32().ToString();
            if (operatorCount != null)
            {
                label13.Text = operatorCount;
            }
            string? complexityRatio = root.GetProperty("complexity_ratio").GetDouble().ToString();
            if (complexityRatio != null)
            {
                label14.Text = complexityRatio;
            }
            // {"condition_count":8,"max_nesting_level":4,"ovperator_count":20,"complexity_ratio":0.4}
        }
    }
}
