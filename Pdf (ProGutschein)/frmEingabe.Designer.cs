namespace PrintLayout.Text
{
    partial class frmEingabe
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
            this.richText = new System.Windows.Forms.RichTextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericSize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFont = new System.Windows.Forms.Button();
            this.txtFontColor = new System.Windows.Forms.TextBox();
            this.txtFontName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkBold = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSize)).BeginInit();
            this.SuspendLayout();
            // 
            // richText
            // 
            this.richText.BackColor = System.Drawing.Color.LightYellow;
            this.richText.BulletIndent = 1;
            this.richText.HideSelection = false;
            this.richText.Location = new System.Drawing.Point(16, 22);
            this.richText.Name = "richText";
            this.richText.ShowSelectionMargin = true;
            this.richText.Size = new System.Drawing.Size(516, 219);
            this.richText.TabIndex = 0;
            this.richText.Text = "";
            this.richText.WordWrap = false;
            this.richText.TextChanged += new System.EventHandler(this.richText_TextChanged);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(432, 250);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "&Übernehmen";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richText);
            this.groupBox1.Controls.Add(this.btnOk);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(550, 283);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericSize);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnFont);
            this.groupBox2.Controls.Add(this.txtFontColor);
            this.groupBox2.Controls.Add(this.txtFontName);
            this.groupBox2.Location = new System.Drawing.Point(12, 301);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(197, 130);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Schrift   ";
            // 
            // numericSize
            // 
            this.numericSize.Location = new System.Drawing.Point(114, 46);
            this.numericSize.Name = "numericSize";
            this.numericSize.Size = new System.Drawing.Size(70, 20);
            this.numericSize.TabIndex = 8;
            this.numericSize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "FontColor:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "FontSize:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "FontName:";
            // 
            // btnFont
            // 
            this.btnFont.Location = new System.Drawing.Point(95, 97);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(89, 23);
            this.btnFont.TabIndex = 4;
            this.btnFont.Text = "&Übernehmen";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // txtFontColor
            // 
            this.txtFontColor.Location = new System.Drawing.Point(84, 71);
            this.txtFontColor.Name = "txtFontColor";
            this.txtFontColor.Size = new System.Drawing.Size(100, 20);
            this.txtFontColor.TabIndex = 2;
            this.txtFontColor.Text = "Black";
            // 
            // txtFontName
            // 
            this.txtFontName.Location = new System.Drawing.Point(84, 19);
            this.txtFontName.Name = "txtFontName";
            this.txtFontName.Size = new System.Drawing.Size(100, 20);
            this.txtFontName.TabIndex = 0;
            this.txtFontName.Text = "Arial";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(240, 320);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(322, 68);
            this.label4.TabIndex = 4;
            this.label4.Text = "Damit ein Text einen definierten Stil erhält muss dieser Text, vor der \"Übernahme" +
                "\", selektiert werden!";
            // 
            // chkBold
            // 
            this.chkBold.AutoSize = true;
            this.chkBold.Location = new System.Drawing.Point(215, 413);
            this.chkBold.Name = "chkBold";
            this.chkBold.Size = new System.Drawing.Size(47, 17);
            this.chkBold.TabIndex = 5;
            this.chkBold.Text = "Bold";
            this.chkBold.UseVisualStyleBackColor = true;
            this.chkBold.CheckedChanged += new System.EventHandler(this.chkBold_CheckedChanged);
            // 
            // frmEingabe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 442);
            this.Controls.Add(this.chkBold);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEingabe";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Eingabe Text";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richText;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFont;
        private System.Windows.Forms.TextBox txtFontColor;
        private System.Windows.Forms.TextBox txtFontName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkBold;
    }
}