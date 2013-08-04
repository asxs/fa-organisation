using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KES.Gutschein
{
    public partial class ProgressForm : Form
    {
        public bool IsActive = true;

        public ProgressForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.IsActive = false;
        }
    }
}
