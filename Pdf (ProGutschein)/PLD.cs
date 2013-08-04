using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PrintLayout.Berechnungen;
using PrintLayout.Designer_XML;
using PrintLayoutDesigner.CustomControl;

namespace PrintLayout
{
    public partial class frmPLD : Form
    {
        public enum Definitions
        {
            Header
        };

        private DIN din = null;
        private Definitions def = default(Definitions);
        private Panel dinPanel = null;
        private int HeaderCount = 0;
        private CustomPDF ccPDF = null;
        private Graphics g = null;
        private int X = 0;
        private int Y = 0;
        private bool IsMouseDown = false;

        public frmPLD()
        {
            InitializeComponent();

            this.dinPanel = new Panel();
            this.dinPanel.MouseDoubleClick += new MouseEventHandler(dinPanel_MouseDoubleClick);
            this.numericUpX.MouseDown += new MouseEventHandler(numericUpX_MouseDown);
            this.numericUpY.MouseDown += new MouseEventHandler(numericUpY_MouseDown);

            //this.btnUeberschrift = new Button();
        }

        void numericUpY_MouseDown(object sender, MouseEventArgs e)
        {
            focusButton.Location = new Point(focusButton.Location.X, (int)numericUpY.Value);
        }

        void numericUpX_MouseDown(object sender, MouseEventArgs e)
        {
            focusButton.Location = new Point((int)numericUpX.Value, focusButton.Location.Y);
        }

        private void frmPLD_Load(object sender, EventArgs e)
        {
            this.din = new DIN();
            this.din.DrawPanelHF(72, 0.5, DINA.DINA4, this.DINContainer, ref this.dinPanel);
        }

        private void radioA4_CheckedChanged(object sender, EventArgs e)
        {
            this.DINContainer.Controls.RemoveByKey("DIN_CONTAINER");
            if (radioA4.Checked && radioHoch.Checked)
                this.din.DrawPanelHF(72, 0.5, DINA.DINA4, this.DINContainer, ref this.dinPanel);
            else if (radioA4.Checked && radioQuer.Checked)
                this.din.DrawPanelQF(72, 0.5, DINA.DINA4, this.DINContainer, ref this.dinPanel);
        }

        private void radioA5_CheckedChanged(object sender, EventArgs e)
        {
            this.DINContainer.Controls.RemoveByKey("DIN_CONTAINER");
            if (radioA5.Checked && radioHoch.Checked)
                this.din.DrawPanelHF(72, 0.5, DINA.DINA3, this.DINContainer, ref this.dinPanel);
            else if (radioA5.Checked && radioQuer.Checked)
                this.din.DrawPanelQF(72, 0.5, DINA.DINA3, this.DINContainer, ref this.dinPanel);
        }

        private void radioHoch_CheckedChanged(object sender, EventArgs e)
        {
            this.DINContainer.Controls.RemoveByKey("DIN_CONTAINER");
            if (radioHoch.Checked)
            {
                if (radioA4.Checked)
                    this.din.DrawPanelHF(72, 0.5, DINA.DINA4, this.DINContainer, ref this.dinPanel);
                else if (radioA5.Checked)
                    this.din.DrawPanelHF(72, 0.5, DINA.DINA3, this.DINContainer, ref this.dinPanel);
            }
        }

        private void radioQuer_CheckedChanged(object sender, EventArgs e)
        {
            if (radioQuer.Checked)
            {
                this.DINContainer.Controls.RemoveByKey("DIN_CONTAINER");
                if (radioA4.Checked)
                    this.din.DrawPanelQF(72, 0.5, DINA.DINA4, this.DINContainer, ref this.dinPanel);
                else if (radioA5.Checked)
                    this.din.DrawPanelQF(72, 0.5, DINA.DINA3, this.DINContainer, ref this.dinPanel);
            }
        }

        private void tsHeader_Click(object sender, EventArgs e)
        {
            this.def = Definitions.Header;
        }

        private IList<IList<string>> lstTexts = new List<IList<string>>();

        private void dinPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {

                this.mouseclick = false;
                this.ccPDF = new CustomPDF();
                this.ccPDF.Location = new Point(e.X, e.Y);
                //this.numericUpX.Value = e.X;
                //this.numericUpY.Value = e.Y;
                this.ccPDF.Text = "Text";
                this.ccPDF.Name = "Header_" + HeaderCount++.ToString();
                this.ccPDF.DoubleClick += new EventHandler(btnUeberschrift_DoubleClick);
                this.ccPDF.MouseMove += new MouseEventHandler(btnUeberschrift_MouseMove);
                this.ccPDF.MouseDown += new MouseEventHandler(btnUeberschrift_MouseDown);
                this.ccPDF.MouseUp += new MouseEventHandler(btnUeberschrift_MouseUp);
                this.ccPDF.Paint += new PaintEventHandler(btnUeberschrift_Paint);
                this.ccPDF.MouseClick += new MouseEventHandler(btnUeberschrift_MouseClick);
                this.dinPanel.Paint += new PaintEventHandler(dinPanel_Paint);
                this.dinPanel.Controls.Add(ccPDF);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        IList<CustomPDF> PDFContainer = new List<CustomPDF>();
        bool mouseclick = false;
        void btnUeberschrift_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.focusButton = sender as CustomPDF;


                    if (this.focusButton is CustomPDF)
                    {
                        if ((this.lstAvailable.Items.IndexOf(this.focusButton.Name)) < 0)
                        {
                            this.lstAvailable.Items.Add(this.focusButton.Name);
                            this.PDFContainer.Add(this.focusButton);

                        }
                    }
                }
                else
                {
                    this.mouseclick = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void btnUeberschrift_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                CustomPDF btn = sender as CustomPDF;
                if (btn.Location.X > -1)
                    this.numericUpX.Value = btn.Location.X;

                if (btn.Location.Y > -1)
                    this.numericUpY.Value = btn.Location.Y;

                PdfSharp.Drawing.XUnit unitX = PdfSharp.Drawing.XUnit.FromPoint(btn.Location.X) + PdfSharp.Drawing.XUnit.FromPoint(btn.Location.X);
                PdfSharp.Drawing.XUnit unitY = PdfSharp.Drawing.XUnit.FromPoint(btn.Location.Y) + PdfSharp.Drawing.XUnit.FromPoint(btn.Location.Y);

                this.txtBreiteCm.Text = unitX.Centimeter.ToString();
                this.txtHoeheCm.Text = unitY.Centimeter.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void dinPanel_Paint(object sender, PaintEventArgs e)
        {
            if (IsMoving)
            {
                //this.dinPanel.Invalidate(new Rectangle(10, 10, 100, 25));
                e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                e.Graphics.DrawString("X: " + MouseX.ToString() + " Y: " + MouseY.ToString(), new Font("Arial", 10.0f, FontStyle.Bold), Brushes.Black, new PointF(10, 10));

                e.Graphics.DrawLine(new Pen(Brushes.Red), new Point(this.focusButton.Location.X, 0), new Point(this.focusButton.Location.X, this.focusButton.Location.Y));
                e.Graphics.DrawLine(new Pen(Brushes.Red), new Point(0, this.focusButton.Location.Y), new Point(this.focusButton.Location.X, this.focusButton.Location.Y));
                e.Graphics.DrawLine(new Pen(Brushes.Red), new Point(this.focusButton.Location.X, this.dinPanel.Height), new Point(this.focusButton.Location.X, this.focusButton.Location.Y));

                e.Graphics.DrawLine(new Pen(Brushes.Red), new Point(this.focusButton.Location.X+this.focusButton.Width, 0), new Point(this.focusButton.Location.X+this.focusButton.Width, this.focusButton.Location.Y));
                e.Graphics.DrawLine(new Pen(Brushes.Red), new Point(0, this.focusButton.Location.Y+this.focusButton.Height), new Point(this.focusButton.Location.X, this.focusButton.Location.Y+this.focusButton.Height));
                e.Graphics.DrawLine(new Pen(Brushes.Red), new Point(this.focusButton.Location.X+this.focusButton.Width, this.dinPanel.Height), new Point(this.focusButton.Location.X+this.focusButton.Width, this.focusButton.Location.Y));

                e.Graphics.DrawLine(new Pen(Brushes.Red), new Point(this.focusButton.Location.X + this.focusButton.Width, this.focusButton.Location.Y), new Point(this.dinPanel.Width, this.focusButton.Location.Y));
                e.Graphics.DrawLine(new Pen(Brushes.Red), new Point(this.focusButton.Location.X + this.focusButton.Width, this.focusButton.Location.Y+this.focusButton.Height), new Point(this.dinPanel.Width, this.focusButton.Location.Y+this.focusButton.Height));

                e.Graphics.FillEllipse(Brushes.LightCoral, new Rectangle(this.focusButton.Location.X - (this.focusButton.Width / 2), this.focusButton.Location.Y - (this.focusButton.Height / 2), 50, 50));

                PdfSharp.Drawing.XUnit unitX = PdfSharp.Drawing.XUnit.FromPoint(MouseX) + PdfSharp.Drawing.XUnit.FromPoint(MouseX);
                PdfSharp.Drawing.XUnit unitY = PdfSharp.Drawing.XUnit.FromPoint(MouseY) + PdfSharp.Drawing.XUnit.FromPoint(MouseY);
                this.txtBreiteCm.Text = unitX.Centimeter.ToString();
                this.txtHoeheCm.Text = unitY.Centimeter.ToString();
            }

            if (pdfItem != null)
            {
                e.Graphics.FillRectangle(Brushes.DarkOrange, new Rectangle(this.pdfItem.Location.X - (this.pdfItem.Width / 2), this.pdfItem.Location.Y - (this.pdfItem.Height / 2), 50, 50));
                //System.Threading.Thread.Sleep(500);
                pdfItem = null;
            }
        }
        float MouseX, MouseY;
        void btnUeberschrift_Paint(object sender, PaintEventArgs e)
        {
            if (IsMoving)
            {
                //this.dinPanel.Invalidate();
            }
        }

        private bool Drag = false;

        private void btnUeberschrift_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                btn2 = sender as CustomPDF;
                if (e.Button == MouseButtons.Left)
                {
                    this.dinPanel.Invalidate();
                    this.dinPanel.BackColor = SystemColors.Control;

                    this.Drag = false;
                    Cursor.Clip = System.Drawing.Rectangle.Empty;
                    //btn.BackColor = SystemColors.Control;
                    //btn.Focus();
                    btn2.Invalidate();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    this.contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        CustomPDF focusButton = new CustomPDF();

        private void btnUeberschrift_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    focusButton = sender as CustomPDF;


                    this.Drag = true;
                    //this.X = e.X;
                    //this.Y = e.Y;
                    this.X = -e.X;
                    this.Y = -e.Y;
                    int clipLeft = this.dinPanel.PointToClient(MousePosition).X - focusButton.Location.X;
                    int clipRight = this.dinPanel.PointToClient(MousePosition).Y - focusButton.Location.Y;
                    int clipWidth = this.dinPanel.ClientSize.Width - (focusButton.Width - clipLeft);
                    int clipHeight = this.dinPanel.ClientSize.Height - (focusButton.Height - clipRight);
                    Cursor.Clip = this.dinPanel.RectangleToClient(new Rectangle(clipLeft, clipRight, clipWidth, clipHeight));

                    //btn.Focus();
                    //this.numericUpX.Value = clipLeft;
                    //this.numericUpY.Value = clipRight;
                    focusButton.Invalidate();
                    //this.dinPanel.Invalidate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool IsMoving = false;

        private void btnUeberschrift_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    CustomPDF btn = sender as CustomPDF;

                    if (this.Drag)
                    {
                        if (btn.Name == focusButton.Name)
                        {
                            IsMoving = true;
                            Point p = new Point();
                            p = this.dinPanel.PointToClient(MousePosition);
                            //p = this.dinPanel.PointToScreen(MousePosition);
                            p.Offset(this.X, this.Y);
                            btn.Location = p;
                            this.MouseX = p.X;
                            this.MouseY = p.Y;


                            if (p.X > -1)
                                this.numericUpX.Value = p.X;

                            if (p.Y > -1)
                                this.numericUpY.Value = p.Y;

                            this.dinPanel.Invalidate();
                            this.dinPanel.BackColor = Color.Snow;
                            //btn.BackColor = Color.OrangeRed;
                            //btn.Focus();
                        }
                    }
                }
                else
                {
                    IsMoving = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string fileName = string.Empty;

        private void layoutSpeichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveLayout();
        }

        private void SaveLayout()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AutoUpgradeEnabled = true;
            sfd.CheckPathExists = true;
            sfd.Filter = "Designer INI|*.ini";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                this.fileName = sfd.FileName;

                IList<DesignerXMLControls> lstControls = new List<DesignerXMLControls>();

                for (int i = 0; i < this.dinPanel.Controls.Count; ++i)
                {
                    DesignerXMLControls cntrls = new DesignerXMLControls();
                    DesignerXMLControls.Size size = new DesignerXMLControls.Size();
                    DesignerXMLControls.Location location = new DesignerXMLControls.Location();

                    size.Width = this.dinPanel.Controls[i].Size.Width;
                    size.Height = this.dinPanel.Controls[i].Size.Height;
                    cntrls.ControlSize = size;

                    location.X = this.dinPanel.Controls[i].Location.X;
                    location.Y = this.dinPanel.Controls[i].Location.Y;
                    cntrls.ControlLocation = location;

                    cntrls.Section = this.dinPanel.Controls[i].Name;

                    lstControls.Add(cntrls);
                }

                if (System.IO.File.Exists(this.fileName))
                    System.IO.File.Delete(this.fileName);

                MyClasses.IniReader iniReader = new MyClasses.IniReader(this.fileName);

                if (this.radioA4.Checked)
                    iniReader.Write("Designer", "Format.DIN", "A4");
                else if (this.radioA5.Checked)
                    iniReader.Write("Designer", "Format.DIN", "A5");

                if (this.radioHoch.Checked)
                    iniReader.Write("Designer", "Format", "H");
                else if (this.radioQuer.Checked)
                    iniReader.Write("Designer", "Format", "Q");

                for (int i = 0; i < lstControls.Count; ++i)
                {
                    iniReader.Write(lstControls[i].Section, "Location.X", lstControls[i].ControlLocation.X.ToString());
                    iniReader.Write(lstControls[i].Section, "Location.Y", lstControls[i].ControlLocation.Y.ToString());

                    iniReader.Write(lstControls[i].Section, "Size.Width", lstControls[i].ControlSize.Width.ToString());
                    iniReader.Write(lstControls[i].Section, "Size.Height", lstControls[i].ControlSize.Height.ToString());

                    for (int j = 0; j < this.lstTexts.Count; ++j)
                    {
                        if (this.lstTexts[j][0] == lstControls[i].Section)
                        {
                            iniReader.Write(lstControls[i].Section, "Text", this.lstTexts[j][1]);
                        }
                    }
                }
            }
        }

        private void layoutLadenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadLayout();
        }

        private void LoadLayout()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AutoUpgradeEnabled = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "Designer INI|*.ini";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.fileName = ofd.FileName;

                    this.dinPanel.Controls.Clear();

                    MyClasses.IniReader iniReader = new MyClasses.IniReader(this.fileName);

                    string DINA = iniReader.ReadString("Designer", "Format.DIN", string.Empty);
                    if (DINA == "A4")
                        this.radioA4.Checked = true;
                    else if (DINA == "A5")
                        this.radioA5.Checked = true;

                    this.lstTexts.Clear();
                    this.dinPanel.Controls.Clear();
                    System.Collections.ArrayList arrayList = iniReader.GetSectionNames();
                    for (int i = 0; i < arrayList.Count; ++i)
                    {
                        CustomPDF btn = new CustomPDF();
                        btn.Location = new Point(iniReader.ReadInteger(arrayList[i].ToString(), "Location.X", 0), iniReader.ReadInteger(arrayList[i].ToString(), "Location.Y", 0));
                        btn.Size = new Size(27, 25);
                        IList<string> lst = new List<string>();
                        lst.Add(btn.Name = arrayList[i].ToString());
                        lst.Add(iniReader.ReadString(arrayList[i].ToString(), "Text"));
                        this.lstTexts.Add(lst);
                        btn.DoubleClick += new EventHandler(btnUeberschrift_DoubleClick);
                        btn.MouseMove += new MouseEventHandler(btnUeberschrift_MouseMove);
                        btn.MouseDown += new MouseEventHandler(btnUeberschrift_MouseDown);
                        btn.MouseUp += new MouseEventHandler(btnUeberschrift_MouseUp);
                        btn.Paint += new PaintEventHandler(btnUeberschrift_Paint);
                        btn.MouseClick += new MouseEventHandler(btnUeberschrift_MouseClick);
                        this.dinPanel.Paint += new PaintEventHandler(dinPanel_Paint);
                        btn.Show();
                        this.dinPanel.Controls.Add(btn);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.richIni.Clear();

            if (System.IO.File.Exists(this.fileName))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(this.fileName);
                while (sr.Peek() > -1)
                {
                    this.richIni.AppendText(sr.ReadLine() + "\r\n");
                }

                sr.Close();
                sr.Dispose();
            }
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            finally
            {
                this.Dispose();
            }
        }

        private void speichernToolStripButton_Click(object sender, EventArgs e)
        {
            SaveLayout();
        }

        private void öffnenToolStripButton_Click(object sender, EventArgs e)
        {
            LoadLayout();
        }

        private void neuToolStripButton_Click(object sender, EventArgs e)
        {
            this.lstTexts.Clear();
            this.HeaderCount = 0;
            this.dinPanel.Controls.Clear();
        }

        private void druckenToolStripButton_Click(object sender, EventArgs e)
        {
            PDF.IPDF pdf = new PDF.CreatePDF();

            pdf.Create();
            pdf.Save(string.Empty);

            System.Diagnostics.Process.Start("H:\test.pdf");
        }

        private void btnEinr_Click(object sender, EventArgs e)
        {

        }
        
        private void numericUpX_ValueChanged(object sender, EventArgs e)
        {
            if (IsMoving)
                focusButton.Location = new Point((int)numericUpX.Value, focusButton.Location.Y);
        }

        private void numericUpY_ValueChanged(object sender, EventArgs e)
        {
            if (IsMoving)
                focusButton.Location = new Point(focusButton.Location.X, (int)numericUpY.Value);
        }

        private void numericUpX_KeyDown(object sender, KeyEventArgs e)
        {
            focusButton.Location = new Point((int)numericUpX.Value, focusButton.Location.Y);
        }

        private void numericUpY_Click(object sender, EventArgs e)
        {
            focusButton.Location = new Point(focusButton.Location.X, (int)numericUpY.Value);
        }

        private void numericUpX_Click(object sender, EventArgs e)
        {
            focusButton.Location = new Point((int)numericUpX.Value, focusButton.Location.Y);
        }

        CustomPDF btn2;
        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintLayout.Text.frmEingabe EingabeText = new PrintLayout.Text.frmEingabe();
            if (this.lstTexts.Count > 0)
            {
                for (int i = 0; i < this.lstTexts.Count; ++i)
                {
                    if (this.lstTexts[i][0] == btn2.Name)
                        EingabeText.TextValue = this.lstTexts[i][1].Replace("#", "\n").Replace("\\T#", "\t");
                }
            }
            if (EingabeText.ShowDialog() == DialogResult.OK)
            {
                string Text = EingabeText.TextValue;

                IList<string> lst = new List<string>();

                lst.Add(btn2.Name);
                lst.Add(Text.Replace("\n", "#").Replace("\t", "\\T#"));

                bool b = false;
                for (int i = 0; i < this.lstTexts.Count; ++i)
                {
                    if (this.lstTexts[i][0] == btn2.Name)
                    {
                        b = true;
                        this.lstTexts[i] = lst;
                    }
                }

                if (!b)
                    this.lstTexts.Add(lst);
            }
        }

        private void löschenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.lstTexts.Count; ++i)
            {
                if (this.lstTexts[i][0] == this.focusButton.Name)
                {
                    this.lstTexts.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < this.lstAvailable.Items.Count; ++i)
            {
                if (this.lstAvailable.Items[i].ToString() == this.focusButton.Name)
                {
                    this.lstAvailable.Items.RemoveAt(i);
                    break;
                }
            }

            for (int i = 0; i < this.PDFContainer.Count; ++i)
            {
                if (this.PDFContainer[i].Name == this.focusButton.Name)
                {
                    this.PDFContainer.RemoveAt(i);
                    break;
                }
            }
            

            this.dinPanel.Controls.RemoveByKey(this.focusButton.Name);
        }

        CustomPDF pdfItem = null;

        private void lstAvailable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstAvailable.SelectedItem != null)
            {
                pdfItem = this.dinPanel.Controls[this.lstAvailable.SelectedItem.ToString()] as CustomPDF;
                this.dinPanel.Invalidate();
            }
        }

        private void listeLöschenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.PDFContainer.Clear();
            this.lstAvailable.Items.Clear();
        }

        private void btnH_Click(object sender, EventArgs e)
        {
            this.dinPanel.Invalidate();
            if (this.PDFContainer.Count > 1)
            {
                CustomPDF first = this.PDFContainer[0];
                for (int i = 1; i < this.PDFContainer.Count; ++i)
                {
                    this.dinPanel.Controls[this.PDFContainer[i].Name].Location = new Point(first.Location.X, this.dinPanel.Controls[this.PDFContainer[i].Name].Location.Y);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("Diese Funktion braucht min. 2 Elemente", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnV_Click(object sender, EventArgs e)
        {
            this.dinPanel.Invalidate();
            if (this.PDFContainer.Count > 1)
            {
                CustomPDF first = this.PDFContainer[0];
                for (int i = 1; i < this.PDFContainer.Count; ++i)
                {
                    this.dinPanel.Controls[this.PDFContainer[i].Name].Location = new Point(this.dinPanel.Controls[this.PDFContainer[i].Name].Location.X, first.Location.Y);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("Diese Funktion braucht min. 2 Elemente", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void rDesign_CheckedChanged(object sender, EventArgs e)
        {
            

        }

        private void rPreview_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.lstTexts.Count; ++i)
            {
                for (int j = 0; j < this.PDFContainer.Count; ++j)
                {
                    if (this.lstTexts[i][0] == this.PDFContainer[j].Name)
                    {
                        this.PDFContainer[j].Controls["lblText"].Text = this.lstTexts[i][1];
                        this.PDFContainer[j].BackgroundImage = null;
                        this.PDFContainer[j].BackColor = SystemColors.Control;
                        Graphics g = Graphics.FromHwnd(this.PDFContainer[j].Controls["lblText"].Handle);
                        this.PDFContainer[j].Controls["lblText"].Width = ((int)g.MeasureString(this.lstTexts[i][1], new Font("Arial", 10.0f)).Width / 2);
                        this.PDFContainer[j].Controls["lblText"].Height = ((int)g.MeasureString(this.lstTexts[i][1], new Font("Arial", 10.0f)).Height / 2);
                        this.PDFContainer[j].Controls["lblText"].Visible = true;
                        this.dinPanel.Controls[this.lstTexts[i][0]].Width = ((int)g.MeasureString(this.lstTexts[i][1], new Font("Arial", 10.0f)).Width / 2);
                        this.dinPanel.Controls[this.lstTexts[i][0]].Height = ((int)g.MeasureString(this.lstTexts[i][1], new Font("Arial", 10.0f)).Height / 2);
                    }
                }
            }
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
