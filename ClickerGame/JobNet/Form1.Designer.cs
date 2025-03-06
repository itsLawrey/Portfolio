namespace JobNet
{
    partial class Form1
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
            components = new System.ComponentModel.Container();
            buttonCookie = new Button();
            labelMoney = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            labelUpgrade = new Label();
            buttonUpgrade = new Button();
            labelIncRate = new Label();
            SuspendLayout();
            // 
            // buttonCookie
            // 
            buttonCookie.Location = new Point(313, 171);
            buttonCookie.Name = "buttonCookie";
            buttonCookie.Size = new Size(140, 142);
            buttonCookie.TabIndex = 0;
            buttonCookie.Text = "Cookie";
            buttonCookie.UseVisualStyleBackColor = true;
            buttonCookie.Click += button1_Click;
            // 
            // labelMoney
            // 
            labelMoney.AutoSize = true;
            labelMoney.Font = new Font("Segoe UI", 90F);
            labelMoney.Location = new Point(248, 9);
            labelMoney.Name = "labelMoney";
            labelMoney.Size = new Size(306, 159);
            labelMoney.TabIndex = 1;
            labelMoney.Text = "#.##";
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // labelUpgrade
            // 
            labelUpgrade.AutoSize = true;
            labelUpgrade.Location = new Point(673, 212);
            labelUpgrade.Name = "labelUpgrade";
            labelUpgrade.Size = new Size(14, 15);
            labelUpgrade.TabIndex = 2;
            labelUpgrade.Text = "#";
            // 
            // buttonUpgrade
            // 
            buttonUpgrade.Location = new Point(592, 208);
            buttonUpgrade.Name = "buttonUpgrade";
            buttonUpgrade.Size = new Size(75, 23);
            buttonUpgrade.TabIndex = 3;
            buttonUpgrade.Text = "Upgrade";
            buttonUpgrade.UseVisualStyleBackColor = true;
            buttonUpgrade.Click += buttonUpgrade_Click;
            // 
            // labelIncRate
            // 
            labelIncRate.AutoSize = true;
            labelIncRate.Location = new Point(363, 153);
            labelIncRate.Name = "labelIncRate";
            labelIncRate.Size = new Size(81, 15);
            labelIncRate.TabIndex = 4;
            labelIncRate.Text = "Income: # cps";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 493);
            Controls.Add(labelIncRate);
            Controls.Add(buttonUpgrade);
            Controls.Add(labelUpgrade);
            Controls.Add(labelMoney);
            Controls.Add(buttonCookie);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonCookie;
        private Label labelMoney;
        private System.Windows.Forms.Timer timer1;
        private Label labelUpgrade;
        private Button buttonUpgrade;
        private Label labelIncRate;
    }
}
