namespace BlackHole.View
{
    partial class GameCell
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGameField = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGameField
            // 
            this.btnGameField.BackgroundImage = global::BlackHole.Resources.Blue;
            this.btnGameField.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnGameField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGameField.Location = new System.Drawing.Point(0, 0);
            this.btnGameField.Name = "btnGameField";
            this.btnGameField.Size = new System.Drawing.Size(150, 150);
            this.btnGameField.TabIndex = 0;
            this.btnGameField.UseVisualStyleBackColor = true;
            this.btnGameField.Click += new System.EventHandler(this.btnGameField_Click);
            // 
            // GameCell
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnGameField);
            this.Name = "GameCell";
            this.ResumeLayout(false);

        }

        #endregion

        public Button btnGameField;
    }
}
