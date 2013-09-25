using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace IxSApp
{
    public partial class UI : Form
    {
        public UI()
        {
            InitializeComponent();
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.Bounds);
            e.Graphics.DrawString(e.Header.Text, e.Font, Brushes.Black, new PointF(3, 7));
        }

        private void UI_Load(object sender, EventArgs e)
        {
            //foreach (TreeNode item in treeXsa.Nodes)
            //    item.ExpandAll();
        }
    }
}
