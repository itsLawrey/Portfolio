namespace BlackHole.View
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.mainMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.easyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelWelcome = new System.Windows.Forms.Label();
            this.gamePanel = new System.Windows.Forms.TableLayoutPanel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip.SuspendLayout();
            this.gamePanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1542, 49);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "Menu";
            // 
            // mainMenuItem
            // 
            this.mainMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveGameMenuItem,
            this.loadGameMenuItem,
            this.newGameMenuItem,
            this.exitGameMenuItem});
            this.mainMenuItem.Name = "mainMenuItem";
            this.mainMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.mainMenuItem.Size = new System.Drawing.Size(119, 45);
            this.mainMenuItem.Text = "Menu";
            // 
            // saveGameMenuItem
            // 
            this.saveGameMenuItem.Name = "saveGameMenuItem";
            this.saveGameMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveGameMenuItem.Size = new System.Drawing.Size(442, 54);
            this.saveGameMenuItem.Text = "Save Game";
            this.saveGameMenuItem.Click += new System.EventHandler(this.saveGameMenuItem_Click);
            // 
            // loadGameMenuItem
            // 
            this.loadGameMenuItem.Name = "loadGameMenuItem";
            this.loadGameMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadGameMenuItem.Size = new System.Drawing.Size(442, 54);
            this.loadGameMenuItem.Text = "Load Game ";
            this.loadGameMenuItem.Click += new System.EventHandler(this.loadGameMenuItem_Click);
            // 
            // newGameMenuItem
            // 
            this.newGameMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.easyToolStripMenuItem,
            this.mediumToolStripMenuItem,
            this.largeToolStripMenuItem});
            this.newGameMenuItem.Name = "newGameMenuItem";
            this.newGameMenuItem.Size = new System.Drawing.Size(442, 54);
            this.newGameMenuItem.Text = "New Game";
            // 
            // easyToolStripMenuItem
            // 
            this.easyToolStripMenuItem.Name = "easyToolStripMenuItem";
            this.easyToolStripMenuItem.Size = new System.Drawing.Size(383, 54);
            this.easyToolStripMenuItem.Text = "Small (5 x 5)";
            this.easyToolStripMenuItem.Click += new System.EventHandler(this.easyToolStripMenuItem_Click);
            // 
            // mediumToolStripMenuItem
            // 
            this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
            this.mediumToolStripMenuItem.Size = new System.Drawing.Size(383, 54);
            this.mediumToolStripMenuItem.Text = "Medium (7 x 7)";
            this.mediumToolStripMenuItem.Click += new System.EventHandler(this.mediumToolStripMenuItem_Click);
            // 
            // largeToolStripMenuItem
            // 
            this.largeToolStripMenuItem.Name = "largeToolStripMenuItem";
            this.largeToolStripMenuItem.Size = new System.Drawing.Size(383, 54);
            this.largeToolStripMenuItem.Text = "Large (9 x 9)";
            this.largeToolStripMenuItem.Click += new System.EventHandler(this.largeToolStripMenuItem_Click);
            // 
            // exitGameMenuItem
            // 
            this.exitGameMenuItem.Name = "exitGameMenuItem";
            this.exitGameMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.exitGameMenuItem.Size = new System.Drawing.Size(442, 54);
            this.exitGameMenuItem.Text = "Exit Game";
            this.exitGameMenuItem.Click += new System.EventHandler(this.exitGameMenuItem_Click);
            // 
            // labelWelcome
            // 
            this.labelWelcome.AutoSize = true;
            this.labelWelcome.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelWelcome.Location = new System.Drawing.Point(3, 0);
            this.labelWelcome.Name = "labelWelcome";
            this.labelWelcome.Size = new System.Drawing.Size(732, 54);
            this.labelWelcome.TabIndex = 2;
            this.labelWelcome.Text = "Select file or start new game from menu";
            // 
            // gamePanel
            // 
            this.gamePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gamePanel.ColumnCount = 10;
            this.gamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.gamePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.gamePanel.Controls.Add(this.labelWelcome, 8, 0);
            this.gamePanel.Location = new System.Drawing.Point(12, 52);
            this.gamePanel.Name = "gamePanel";
            this.gamePanel.RowCount = 2;
            this.gamePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gamePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gamePanel.Size = new System.Drawing.Size(1500, 1500);
            this.gamePanel.TabIndex = 4;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "Blackhole gamefile (*.bgf)|*.bgf";
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Title = "Load game";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Blackhole gamefile (*.bgf)|*.bgf";
            this.saveFileDialog.RestoreDirectory = true;
            this.saveFileDialog.Title = "Save game";
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 1562);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1542, 54);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(213, 41);
            this.toolStripStatusLabel.Text = "Current player:";
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1542, 1616);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.gamePanel);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "GameWindow";
            this.Text = "Blackhole Game";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.gamePanel.ResumeLayout(false);
            this.gamePanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem mainMenuItem;
        private ToolStripMenuItem saveGameMenuItem;
        private ToolStripMenuItem loadGameMenuItem;
        private ToolStripMenuItem newGameMenuItem;
        private ToolStripMenuItem easyToolStripMenuItem;
        private ToolStripMenuItem mediumToolStripMenuItem;
        private ToolStripMenuItem largeToolStripMenuItem;
        private Label labelWelcome;
        private TableLayoutPanel gamePanel;
        private ToolStripMenuItem exitGameMenuItem;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
    }
}