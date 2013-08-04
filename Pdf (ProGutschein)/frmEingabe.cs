using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrintLayout.Text
{
    public partial class frmEingabe : Form
    {
        public frmEingabe()
        {
            InitializeComponent();
        }

        public string TextValue
        {
            get
            {
                return this.richText.Text;
            }
            set
            {
                this.richText.Text = value;
            }
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            string RText = this.richText.SelectedText;
            if (string.IsNullOrEmpty(RText))
            {
                MessageBox.Show("Es muss ein Text selektiert werden!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                string FontText = @"<font name='" + this.txtFontName.Text + "' size='" + this.numericSize.Value.ToString() + "' color='" + this.txtFontColor.Text + "'>" + RText + "</font>";
                this.richText.SelectedText = this.richText.SelectedText.Replace(RText, FontText);
            }
        }

        private void chkBold_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBold.Checked)
            {
                string RText = this.richText.SelectedText;
                if (string.IsNullOrEmpty(RText))
                {
                    MessageBox.Show("Es muss ein Text selektiert werden!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    string FontText = @"<b>" + RText + "</b>";
                    this.richText.SelectedText = this.richText.SelectedText.Replace(RText, FontText);

                    this.chkBold.Checked = false;
                }
            }
        }

        private void richText_TextChanged(object sender, EventArgs e)
        {
            //char c = richText.GetCharFromPosition(richText.Cursor.HotSpot);
            //if (c == '\\')
            //{
            //    ListBox lst = new ListBox();
            //    lst.Location = richText.Cursor.HotSpot;
            //    lst.Show();
            //    this.richText.Controls.Add(lst);
            //}
        }
    }
}
