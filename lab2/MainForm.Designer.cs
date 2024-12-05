namespace Jilb
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            browseBtn = new Button();
            codeTextBox = new TextBox();
            startBtn = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label11 = new Label();
            label12 = new Label();
            label13 = new Label();
            label14 = new Label();
            SuspendLayout();
            // 
            // browseBtn
            // 
            browseBtn.Location = new Point(528, 404);
            browseBtn.Name = "browseBtn";
            browseBtn.Size = new Size(174, 29);
            browseBtn.TabIndex = 0;
            browseBtn.Text = "Browse";
            browseBtn.UseVisualStyleBackColor = true;
            browseBtn.Click += browseBtn_Click;
            // 
            // codeTextBox
            // 
            codeTextBox.Location = new Point(12, 12);
            codeTextBox.Multiline = true;
            codeTextBox.Name = "codeTextBox";
            codeTextBox.ScrollBars = ScrollBars.Both;
            codeTextBox.Size = new Size(497, 473);
            codeTextBox.TabIndex = 1;
            // 
            // startBtn
            // 
            startBtn.Location = new Point(528, 456);
            startBtn.Name = "startBtn";
            startBtn.Size = new Size(174, 29);
            startBtn.TabIndex = 2;
            startBtn.Text = "Start";
            startBtn.UseVisualStyleBackColor = true;
            startBtn.Click += startBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(515, 15);
            label1.Name = "label1";
            label1.Size = new Size(118, 20);
            label1.TabIndex = 3;
            label1.Text = "Condition count:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(515, 53);
            label2.Name = "label2";
            label2.Size = new Size(127, 20);
            label2.TabIndex = 4;
            label2.Text = "Max nesting level:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(515, 89);
            label3.Name = "label3";
            label3.Size = new Size(113, 20);
            label3.TabIndex = 5;
            label3.Text = "Operator count:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(515, 122);
            label4.Name = "label4";
            label4.Size = new Size(122, 20);
            label4.TabIndex = 6;
            label4.Text = "Complexity ratio:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(652, 15);
            label11.Name = "label11";
            label11.Size = new Size(0, 20);
            label11.TabIndex = 7;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(652, 53);
            label12.Name = "label12";
            label12.Size = new Size(0, 20);
            label12.TabIndex = 8;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(652, 89);
            label13.Name = "label13";
            label13.Size = new Size(0, 20);
            label13.TabIndex = 9;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(652, 122);
            label14.Name = "label14";
            label14.Size = new Size(0, 20);
            label14.TabIndex = 10;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(714, 497);
            Controls.Add(label14);
            Controls.Add(label13);
            Controls.Add(label12);
            Controls.Add(label11);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(startBtn);
            Controls.Add(codeTextBox);
            Controls.Add(browseBtn);
            Name = "MainForm";
            Text = "Jilb";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button browseBtn;
        private TextBox codeTextBox;
        private Button startBtn;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
    }
}
