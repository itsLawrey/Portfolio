namespace CookieClicker.View
{
    partial class GameWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            labelMoney = new Label();
            labelIncRate = new Label();
            buttonCookie = new Button();
            buttonUpgrade = new Button();
            labelUpgrade = new Label();
            timer1 = new System.Windows.Forms.Timer(components);
            menuStrip1 = new MenuStrip();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            loadGameToolStripMenuItem = new ToolStripMenuItem();
            saveGameToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            statusStrip1 = new StatusStrip();
            toolStripOnlineStatusLabel = new ToolStripStatusLabel();
            toolStripCurrentTimeLabel = new ToolStripStatusLabel();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // labelMoney
            // 
            labelMoney.AutoSize = true;
            labelMoney.Font = new Font("Segoe UI", 90F);
            labelMoney.Location = new Point(199, 33);
            labelMoney.Name = "labelMoney";
            labelMoney.Size = new Size(306, 159);
            labelMoney.TabIndex = 2;
            labelMoney.Text = "#.##";
            // 
            // labelIncRate
            // 
            labelIncRate.AutoSize = true;
            labelIncRate.Location = new Point(290, 177);
            labelIncRate.Name = "labelIncRate";
            labelIncRate.Size = new Size(81, 15);
            labelIncRate.TabIndex = 5;
            labelIncRate.Text = "Income: # cps";
            // 
            // buttonCookie
            // 
            buttonCookie.Location = new Point(260, 237);
            buttonCookie.Name = "buttonCookie";
            buttonCookie.Size = new Size(140, 142);
            buttonCookie.TabIndex = 6;
            buttonCookie.Text = "Cookie";
            buttonCookie.UseVisualStyleBackColor = true;
            buttonCookie.Click += buttonCookie_Click;
            // 
            // buttonUpgrade
            // 
            buttonUpgrade.Location = new Point(519, 210);
            buttonUpgrade.Name = "buttonUpgrade";
            buttonUpgrade.Size = new Size(75, 23);
            buttonUpgrade.TabIndex = 7;
            buttonUpgrade.Text = "Upgrade";
            buttonUpgrade.UseVisualStyleBackColor = true;
            buttonUpgrade.Click += buttonUpgrade_Click;
            // 
            // labelUpgrade
            // 
            labelUpgrade.AutoSize = true;
            labelUpgrade.Location = new Point(600, 214);
            labelUpgrade.Name = "labelUpgrade";
            labelUpgrade.Size = new Size(14, 15);
            labelUpgrade.TabIndex = 8;
            labelUpgrade.Text = "#";
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 9;
            menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripSeparator1, loadGameToolStripMenuItem, saveGameToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(61, 20);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(131, 6);
            // 
            // loadGameToolStripMenuItem
            // 
            loadGameToolStripMenuItem.Name = "loadGameToolStripMenuItem";
            loadGameToolStripMenuItem.Size = new Size(134, 22);
            loadGameToolStripMenuItem.Text = "Load Game";
            loadGameToolStripMenuItem.Click += loadGameToolStripMenuItem_Click;
            // 
            // saveGameToolStripMenuItem
            // 
            saveGameToolStripMenuItem.Name = "saveGameToolStripMenuItem";
            saveGameToolStripMenuItem.Size = new Size(134, 22);
            saveGameToolStripMenuItem.Text = "Save Game";
            saveGameToolStripMenuItem.Click += saveGameToolStripMenuItem_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.Filter = "CookieClicker file (*.ccf)|*.ccf";
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.Filter = "CookieClicker file (*.ccf)|*.ccf";
            saveFileDialog1.Title = "Save Game";
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripOnlineStatusLabel, toolStripCurrentTimeLabel });
            statusStrip1.Location = new Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 22);
            statusStrip1.TabIndex = 10;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripOnlineStatusLabel
            // 
            toolStripOnlineStatusLabel.Name = "toolStripOnlineStatusLabel";
            toolStripOnlineStatusLabel.Size = new Size(72, 17);
            toolStripOnlineStatusLabel.Text = "Last Online: ";
            // 
            // toolStripCurrentTimeLabel
            // 
            toolStripCurrentTimeLabel.Name = "toolStripCurrentTimeLabel";
            toolStripCurrentTimeLabel.Size = new Size(79, 17);
            toolStripCurrentTimeLabel.Text = "Current Time:";
            // 
            // GameWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(labelUpgrade);
            Controls.Add(buttonUpgrade);
            Controls.Add(buttonCookie);
            Controls.Add(labelIncRate);
            Controls.Add(labelMoney);
            Name = "GameWindow";
            Text = "GameWindow";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelMoney;
        private Label labelIncRate;
        private Button buttonCookie;
        private Button buttonUpgrade;
        private Label labelUpgrade;
        private System.Windows.Forms.Timer timer1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem loadGameToolStripMenuItem;
        private ToolStripMenuItem saveGameToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripOnlineStatusLabel;
        private ToolStripStatusLabel toolStripCurrentTimeLabel;
    }
}