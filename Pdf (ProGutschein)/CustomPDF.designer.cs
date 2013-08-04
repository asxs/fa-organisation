namespace PrintLayoutDesigner.CustomControl
{
    partial class CustomPDF
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
            this.lblText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblText
            // 
            this.lblText.BackColor = System.Drawing.Color.Transparent;
            this.lblText.Location = new System.Drawing.Point(3, 6);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(10, 14);
            this.lblText.TabIndex = 0;
            this.lblText.Visible = false;
            // 
            // CustomPDF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            //this.BackgroundImage = global::As.Properties.Resources.pdf;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.lblText);
            this.Name = "CustomPDF";
            this.Size = new System.Drawing.Size(27, 25);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblText;

    }
}
