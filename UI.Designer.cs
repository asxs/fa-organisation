namespace As
{
    partial class UI
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UI));
            this.btnPopTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPopTest
            // 
            this.btnPopTest.Location = new System.Drawing.Point(752, 12);
            this.btnPopTest.Name = "btnPopTest";
            this.btnPopTest.Size = new System.Drawing.Size(107, 25);
            this.btnPopTest.TabIndex = 0;
            this.btnPopTest.Text = "&POP3 - Test";
            this.btnPopTest.UseVisualStyleBackColor = true;
            this.btnPopTest.Click += new System.EventHandler(this.btnPopTest_Click);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 487);
            this.Controls.Add(this.btnPopTest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "UI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Organisation von Bewerbungen - Lars Ulrich Herrmann (c) 2013";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPopTest;
    }
}

