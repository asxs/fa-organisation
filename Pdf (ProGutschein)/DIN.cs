using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintLayout.Berechnungen
{
    public enum DINA
    {
        DINA4,
        DINA3
    };

    public class DIN
    {
        public DIN()
        {

        }

        public void DrawPanelQF(double dpi, double percent, DINA din, System.Windows.Forms.Panel p, ref System.Windows.Forms.Panel dinPanel)
        {
            if (percent == 0)
                percent = 0.5;

            if (dpi == 0)
                dpi = 72;

            double Width = 0;
            double Height = 0;

            if (din == DINA.DINA4)
            {
                Width = 29.7;
                Height = 21.0;
            }
            else if (din == DINA.DINA3)
            {
                Width = 21.0;
                Height = 14.8;
            }

            double drawWidth = (dpi * Width / 2.54) * percent;
            double drawHeight = (dpi * Height / 2.54) * percent;

            dinPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dinPanel.Width = (int)drawWidth;
            dinPanel.Height = (int)drawHeight;
            p.Controls.Add(dinPanel);
        }

        public void DrawPanelHF(double dpi, double percent, DINA din, System.Windows.Forms.Panel p, ref System.Windows.Forms.Panel dinPanel)
        {
            if (percent == 0)
                percent = 0.5;

            if (dpi == 0)
                dpi = 72;

            double Width = 0;
            double Height = 0;

            if (din == DINA.DINA4)
            {
                Width = 21.0;
                Height = 29.7;
            }
            else if (din == DINA.DINA3)
            {
                Width = 14.8;
                Height = 21.0;
            }

            double drawWidth = (dpi * Width / 2.54) * percent;
            double drawHeight = (dpi * Height / 2.54) * percent;

            dinPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dinPanel.Width = (int)drawWidth;
            dinPanel.Height = (int)drawHeight;
            dinPanel.Name = "DIN_CONTAINER";
            p.Controls.Add(dinPanel);
        }
    }
}
