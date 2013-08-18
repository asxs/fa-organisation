using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IxSApp
{
    public partial class Card : UserControl
    {
        public Card()
        {
            InitializeComponent();
        }

        private void Card_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.LightYellow, new Rectangle(1, Height, Width - 1, Height));
        }
    }
}
