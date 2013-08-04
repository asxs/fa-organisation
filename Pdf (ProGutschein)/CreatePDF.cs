using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using DTaus;

namespace PrintLayout.PDF
{
    [ComVisible(true)]
    public interface IPDF
    {
        [DispId(0)]
        string Create();

        [DispId(1)]
        string ShowRueck();

        [DispId(2)]
        void SetCallBack(int Handle);

        [DispId(3)]
        object[] GetReturnValues();

        [DispId(4)]
        void SetValues1(string ObjNr);

        [DispId(5)]
        void Set2EntryForm();

        [DispId(6)]
        void SetValues2(string GutNr);

        [DispId(7)]
        string FileDialog();

        [DispId(8)]
        void SetText4Bank(string Key, string Value);

        [DispId(9)]
        void SetText4KT(string Key, string Value);

        [DispId(10)]
        void DeleteKeys();

        [DispId(11)]
        void Save(string Rechnungsdatei);

        [DispId(12)]
        string GetFileName();

        [DispId(50)]
        [ComVisible(true)]
        void SetBarCode(string CodeNr, int X, int Y);

        [DispId(51)]
        void CreateASatz(string Kennzeichen, string AuftraggeberBLZ, string AuftraggeberName, string AuftraggeberKonto);

        [DispId(52)]
        void CreateCSatz(string Kennzeichen, string AuftraggeberBLZ, string EmpfängerBLZ, string EmpfängerKonto, string BetragInEUR, string EmpfängerName, string AuftraggeberName, string Verwendungszweck, string AuftragKonto, string AuftragBLZ4);

        [DispId(53)]
        void CreateESatz(string SummeKontoNr, string SummeBLZ, string SummeBetrag);

        [DispId(54)]
        void CreateDTAUS(string FileName);

        [DispId(55)]
        void SetRechnung(bool b);

        [DispId(56)]
        void EnableBarCode(bool b);

        [DispId(57)]
        bool InitializePDF();

        [DispId(58)]
        void DisposePDF();

        [DispId(59)]
        void CheckThreads();
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class CreatePDF : IPDF
    {
        private IList<object[,]> SaveingList = null;
        private string FileName = string.Empty;
        //private PdfDocument pdfDocument = null;
        private int Handle = 0;
        private delegate string VOMethod(string Message);
        private object[] o = new object[10000];
        private string ObjNumber = string.Empty;
        //private PrintLayout.Rückläufer.REntry entry = null;
        private IList<string> ListOfObjectNumbers = null;
        private IList<string> ListOfGutscheinNumbers = null;
        private string[] ListOfBank = null;
        private string[] ListOfKt = null;
        private IDictionary<string, string> dictionaryListOfBank = null;
        private IDictionary<string, string> dictionaryListOfKT = null;
        private string DTAUS = string.Empty;
        private DTausParser dTausParser = null;
        private bool Rechnung = false;
        private bool bEnableBarCode = false;
        private int n = 0;

        [DispId(59)]
        public void CheckThreads()
        {
            //while (nThreads != 0)
            //{
            //    System.Threading.Thread.Sleep(5);
            //}
            int z = 0;

            IsThread = true;
            foreach (System.Threading.Thread th in threadPool)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(CreatePDFThread), z++);
                //th.Start(z++);
            }

            while (n < threadPool.Length)
                System.Threading.Thread.Sleep(2);

            Save("");
        }

        public CreatePDF()
        {
            this.SaveingList = new List<object[,]>();
            //this.entry = new PrintLayout.Rückläufer.REntry();

            this.ListOfGutscheinNumbers = new List<string>();
            this.ListOfObjectNumbers = new List<string>();

            this.dictionaryListOfBank = new Dictionary<string, string>();
            this.dictionaryListOfKT = new Dictionary<string, string>();
        }

        [DispId(56)]
        public void EnableBarCode(bool b)
        {
            this.bEnableBarCode = b;
        }

        [DispId(58)]
        public void DisposePDF()
        {

        }

        [DispId(51)]
        public void CreateASatz(string Kennzeichen, string AuftraggeberBLZ, string AuftraggeberName, string AuftraggeberKonto)
        {
            this.dTausParser = new DTausParser();
            //this.DTAUS = this.dTausParser.PrepareASatz("0128", "A", "GK", "25050180", "", "Lars Herrmann", "050509", "", "1900411121", "", "", "1");

            string Date = DateTime.Now.Day.ToString().PadLeft(2, '0') + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Year.ToString().Substring(3).PadLeft(2, '0');
            this.DTAUS = string.Empty;
            this.DTAUS = this.dTausParser.PrepareASatz("0128", "A", Kennzeichen, AuftraggeberBLZ, "", AuftraggeberName, Date, "", AuftraggeberKonto, "", "", "1");
        }

        [DispId(52)]
        public void CreateCSatz(string Kennzeichen, string AuftraggeberBLZ, string EmpfängerBLZ, string EmpfängerKonto, string BetragInEUR, string EmpfängerName, string AuftraggeberName, string Verwendungszweck, string AuftragKonto, string AuftragBLZA4)
        {
            //this.DTAUS += this.dTausParser.PrepareCSatz(
            //    "0187", "C", "25050180", "25050180", "1900411121", "", "51", "000", "", "", "25050180", "1900411121", "121215", "", "Andreas Klocke", "",
            //    "ANDREAS KLOCKE", "TEST", "1", "", "");

            this.DTAUS += this.dTausParser.PrepareCSatz(
                "0187", "C", AuftraggeberBLZ, EmpfängerBLZ, AuftragKonto, "", Kennzeichen, "000", "", "", string.IsNullOrEmpty(AuftragBLZA4) ? EmpfängerBLZ : AuftragBLZA4, EmpfängerKonto, BetragInEUR, "", EmpfängerName, "",
                AuftraggeberName, Verwendungszweck, "1", "", "");
        }

        [DispId(53)]
        public void CreateESatz(string SummeKontoNr, string SummeBLZ, string SummeBetrag)
        {
            //this.DTAUS += this.dTausParser.PrepareESatz("0128", "E", "", "1", "0", "1900411121", "25050180", "121215", "");
            this.DTAUS += this.dTausParser.PrepareESatz("0128", "E", "", "1", "0", SummeKontoNr, SummeBLZ, SummeBetrag, "");
        }

        [DispId(54)]
        public void CreateDTAUS(string FileName)
        {
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(FileName))
            {
                sw.Write(this.DTAUS);
                sw.Close();
            }
        }

        [DispId(55)]
        public void SetRechnung(bool b)
        {
            this.Rechnung = b;
        }

        [DispId(10)]
        public void DeleteKeys()
        {
            this.dictionaryListOfBank.Clear();
            this.dictionaryListOfKT.Clear();
        }

        [DispId(11)]
        public void Save(string Rechnungsdatei)
        {
            //Counter = 0;

            //try
            //{
            //    if (this.sPdfFile != null)
            //    {
            //        if (this.Rechnung)
            //        {
            //            this.sPdfFile = this.FileName = Rechnungsdatei;
            //            this.pdfDocument.Save(this.sPdfFile);
            //        }
            //        else
            //        {
            //            string message = string.Empty;
            //            if (!this.pdfDocument.CanSave(ref message))
            //            {
            //                while (!this.pdfDocument.CanSave(ref message))
            //                {
            //                    this.pdfDocument.Save(this.FileName = this.sPdfFile);
            //                }
            //            }
            //            else
            //                this.pdfDocument.Save(this.FileName = this.sPdfFile);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //}
        }

        [DispId(12)]
        public string GetFileName()
        {
            Counter = 0;
            return this.FileName;
        }

        [DispId(9)]
        public void SetText4KT(string Key, string Value)
        {
            if (!this.dictionaryListOfKT.ContainsKey(Key))
                this.dictionaryListOfKT.Add(new KeyValuePair<string, string>(Key, Value));
        }

        [DispId(8)]
        public void SetText4Bank(string Key, string Value)
        {
            if (!this.dictionaryListOfBank.ContainsKey(Key))
                this.dictionaryListOfBank.Add(new KeyValuePair<string, string>(Key, Value));
        }

        [DispId(7)]
        public string FileDialog()
        {
            string Filename = string.Empty;

            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();

            ofd.AutoUpgradeEnabled = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "Mitarbeiterliste (CSV)|*.txt";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Filename = ofd.FileName;
                return Filename;
            }

            return Filename;
        }

        [DispId(4)]
        public void SetValues1(string ObjNr)
        {
            this.ListOfObjectNumbers.Add(ObjNr);
        }

        [DispId(6)]
        public void SetValues2(string GutNr)
        {
            this.ListOfGutscheinNumbers.Add(GutNr);
        }

        [DispId(5)]
        public void Set2EntryForm()
        {
            //this.entry.GutscheinNumbers = this.ListOfGutscheinNumbers;
            //this.entry.ObjectNumbers = this.ListOfObjectNumbers;
        }

        [DispId(2)]
        public void SetCallBack(int Handle)
        {
            this.Handle = Handle;
        }

        [DispId(1)]
        public string ShowRueck()
        {
            //System.Windows.Forms.DialogResult result = this.entry.ShowDialog();

            //if (result == System.Windows.Forms.DialogResult.Retry)
            //{
            //    VOMethod vo = (VOMethod)Marshal.GetDelegateForFunctionPointer((IntPtr)this.Handle, typeof(VOMethod));
            //    if (vo is VOMethod)
            //    {
            //        string TemporaryReturnString = this.entry.cbObject.Text + "#" + this.entry.cbNumber.Text + "#" +
            //                                this.entry.cbLiter.Text + "#" + this.entry.txtFirma.Text + "#" +
            //                                this.entry.txtGesamt.Text + "#" + this.entry.txtPreis.Text + "#" + this.entry.mDate.SelectionRange.Start.Date.ToString();

            //        vo(TemporaryReturnString);
            //    }

            //    this.entry.GutscheinNumbers.Remove(this.entry.cbNumber.Text);

            //    ShowRueck();
            //}
            //else if (result == System.Windows.Forms.DialogResult.OK)
            //{
            //    VOMethod vo = (VOMethod)Marshal.GetDelegateForFunctionPointer((IntPtr)this.Handle, typeof(VOMethod));
            //    if (vo is VOMethod)
            //    {
            //        string TemporaryReturnString = this.entry.cbObject.Text + "#" + this.entry.cbNumber.Text + "#" +
            //                                this.entry.cbLiter.Text + "#" + this.entry.txtFirma.Text + "#" +
            //                                this.entry.txtGesamt.Text + "#" + this.entry.txtPreis.Text + "#" + this.entry.mDate.SelectionRange.Start.Date.ToString();

            //        vo(TemporaryReturnString);
            //    }
            //}
            //else
            //{

            //}

            return string.Empty;
        }

        [DispId(3)]
        public object[] GetReturnValues()
        {
            object[] o = new object[10000];
            return o;
        }

        //private ProgressForm prog = null;

        [ComVisible(false)]
        private void CreateProgress(object PageCount)
        {
            this.th.Abort();
        }

        private System.Threading.Thread th = null;
        private string sIniFile = string.Empty;
        private string sPdfFile = string.Empty;

        [ComVisible(false)]
        private void ShowDialogs()
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();

            ofd.AutoUpgradeEnabled = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "Designer INI|*.ini";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.sIniFile = ofd.FileName;

                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
                sfd.Filter = "Portable Document (PDF)|*.PDF";
                sfd.CheckPathExists = true;
                sfd.AutoUpgradeEnabled = true;

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.sPdfFile = sfd.FileName;
                }
            }
        }

        private static int Counter = 0;
        private static string DINA = string.Empty;
        //private static XGraphics xGraphics;
        private static IList<IList<float>> lstCoords;
        private static IList<IList<string>> lstText;
        //private static Section sec;
        //private static HTMLParser parser;
        //private static Document doc;
        private string FORMAT;
        private string CodeNr;
        private string BarCodeFile;

        [DispId(50)]
        [ComVisible(true)]
        public void SetBarCode(string CodeNr, int X, int Y)
        {
            this.CodeNr = CodeNr;

            //TECIT.TBarCode.Barcode barCode = new TECIT.TBarCode.Barcode();
            ////barCode.BarcodeType = TECIT.TBarCode.BarcodeType.Code128;
            //barCode.BarcodeType = TECIT.TBarCode.BarcodeType.DataMatrix;
            //barCode.BarcodeType = TECIT.TBarCode.BarcodeType.Ean8;
            //barCode.Data = CodeNr;
            //barCode.BoundingRectangle = new System.Drawing.Rectangle(X, Y, 150, 100);
            //System.Drawing.Bitmap bitmap = barCode.DrawBitmap(150, 100);
            //bitmap.Save(Environment.GetEnvironmentVariable("TEMP") + "\\BARCODE" + this.CodeNr + ".GIF", System.Drawing.Imaging.ImageFormat.Gif);
        }

        //private MigraDoc.Rendering.DocumentRenderer docRenderer = null;
        //private Paragraph para = null;
        //private PdfPage page = null;

        [DispId(57)]
        public bool InitializePDF()
        {
            //if (string.IsNullOrEmpty(this.sIniFile) && string.IsNullOrEmpty(this.sPdfFile))
            //    this.ShowDialogs();

            //if (string.IsNullOrEmpty(this.sIniFile) && string.IsNullOrEmpty(this.sPdfFile))
            //{
            //    return false;
            //}

            //this.FileName = this.sPdfFile;

            //System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;

            ////this.pdfDocument = new PdfDocument();
            ////this.pdfDocument.Options.ColorMode = PdfColorMode.Rgb;
            ////this.pdfDocument.Info.CreationDate = DateTime.Now;
            ////this.pdfDocument.Info.Creator = Environment.UserName;

            //MyClasses.IniReader iniReader = new MyClasses.IniReader(this.sIniFile);

            //DINA = iniReader.ReadString("Designer", "Format.DIN", string.Empty);
            //FORMAT = iniReader.ReadString("Designer", "Format", string.Empty);

            //lstCoords = new List<IList<float>>();
            //lstText = new List<IList<string>>();

            //System.Collections.ArrayList arrayList = iniReader.GetSectionNames();
            //for (int i = 0; i < arrayList.Count; ++i)
            //{
            //    IList<float> lst1 = new List<float>();
            //    IList<string> lst2 = new List<string>();

            //    lst1.Add(iniReader.ReadInteger(arrayList[i].ToString(), "Location.X", 0));
            //    lst1.Add(iniReader.ReadInteger(arrayList[i].ToString(), "Location.Y", 0));
            //    lstCoords.Add(lst1);

            //    lst2.Add(iniReader.ReadString(arrayList[i].ToString(), "Text", string.Empty));
            //    lstText.Add(lst2);
            //}

            //IList<PdfPage> Pages = new List<PdfPage>();
            //doc = new Document();
            //parser = new HTMLParser();
            //sec = doc.AddSection();

            //xGraphics = null;

            //docRenderer = new DocumentRenderer(doc);

            //bw.DoWork += new System.ComponentModel.DoWorkEventHandler(bw_DoWork);
            //bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            //bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bw_ProgressChanged);

            //prog = new ProgressForm();

            //this.pdfDocument.Options.CompressContentStreams = true;

            return true;
        }

        //private PDF pdf = null;

        IList<IDictionary<string, string>> lstThreadDict1 = new List<IDictionary<string, string>>();
        IList<IDictionary<string, string>> lstThreadDict2 = new List<IDictionary<string, string>>();

        bool IsThread = false;

        [System.Runtime.CompilerServices.MethodImpl]
        private void CreatePDFThread(object asyncState)
        {
        //    int iX = 0, iY = 0;
        //    try
        //    {

        //        if (!IsThread)
        //        {
        //            lstThreadDict1.Add(this.dictionaryListOfBank);
        //            lstThreadDict2.Add(this.dictionaryListOfKT);
        //        }
        //        //this.th2 = new System.Threading.Thread(new System.Threading.ThreadStart(Test));
        //        //this.th2.Start();
        //        //if (asyncState != null)
        //        //{
        //            //if (lstThreadDict1[(int)asyncState] != null)
        //            //{
        //            //    if (IsThread)
        //            //    {
        //                    page = this.pdfDocument.AddPage();

        //                    if (DINA == "A4")
        //                        page.Size = PageSize.A4;
        //                    else if (DINA == "A5")
        //                        page.Size = PageSize.A5;

        //                    if (FORMAT == "Q")
        //                        page.Orientation = PageOrientation.Landscape;
        //                    else if (FORMAT == "H")
        //                        page.Orientation = PageOrientation.Portrait;

        //                    xGraphics = XGraphics.FromPdfPage(page);

        //                    //bw.ReportProgress(10);


        //                    for (int j = 0; j < lstCoords.Count; ++j)
        //                    {
        //                        this.pdf = new PDF(this.pdfDocument, page, xGraphics);

        //                        string Text = lstText[j][0];

        //                        foreach (KeyValuePair<string, string> kv in this.dictionaryListOfBank)
        //                            Text = Text.Replace(kv.Key, kv.Value);

        //                        foreach (KeyValuePair<string, string> kv in this.dictionaryListOfKT)
        //                            Text = Text.Replace(kv.Key, kv.Value);

        //                        Text = Text.Replace(@"\{TODAY}", DateTime.Today.Day.ToString().PadLeft(2, '0') + "." + DateTime.Today.Month.ToString().PadLeft(2, '0') + "." + DateTime.Today.Year.ToString());


        //                        IList<HTMLParser.TextStyle> listTextStyle = parser.Load(Text);

        //                        HTMLParser.Lists lists = parser.GetLoadedStyles(listTextStyle, ref Text);

        //                        xGraphics.MUH = PdfSharp.Pdf.PdfFontEncoding.Unicode;
        //                        xGraphics.MFEH = PdfSharp.Pdf.PdfFontEmbedding.Default;

        //                        para = sec.AddParagraph();

        //                        //if (lstCoords[j][0] < XUnit.FromCentimeter(2).Inch)
        //                        //    lstCoords[j][0] += (float)XUnit.FromCentimeter(2).Inch;

        //                        if (Text.Trim().StartsWith(@"\{IMAGE}="))
        //                            para.AddImage(Text.Substring(@"\{IMAGE}=".Length));
        //                        else
        //                            if (!Text.StartsWith(@"\{BARCODE}"))
        //                                pdf.DrawString(lists.Color, lists.Positions, lists.Font, Text, (int)(lstCoords[j][0] / 0.5), (int)(lstCoords[j][1] / 0.5), ref para, ref docRenderer);
        //                            else
        //                            {
        //                                iX = (int)(lstCoords[j][0] / 0.5);
        //                                iY = (int)(lstCoords[j][1] / 0.5);
        //                            }

        //                        if (!string.IsNullOrEmpty(Text.Trim()))
        //                        {
        //                            ParagraphFormat format = new ParagraphFormat();
        //                            format.AddTabStop(Unit.FromCentimeter(24.0));

        //                            para.Format = format;
        //                        }

        //                        if (!Text.StartsWith(@"\{BARCODE}") && !string.IsNullOrEmpty(Text.Trim()))
        //                        {
        //                            if (docRenderer != null && xGraphics != null && para != null)
        //                            {
        //                                docRenderer.PrepareDocument();
        //                                docRenderer.RenderObject(xGraphics, new PdfSharp.Drawing.XUnit(lstCoords[j][0] / 0.5), new PdfSharp.Drawing.XUnit(lstCoords[j][1] / 0.5), "24cm", para);
        //                            }
        //                        }
        //                    }
        //            //    }
        //            //}
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
        //    }

        //    if (this.bEnableBarCode)
        //    {
        //        //this.SetBarCode(CodeNr, (int)(lstCoords[j][0] / 0.5), (int)(lstCoords[j][1] / 0.5));

        //        if (docRenderer != null)
        //        {
        //            if (para != null)
        //            {
        //                para = sec.AddParagraph();
        //                para.AddImage(Environment.GetEnvironmentVariable("TEMP") + "\\BARCODE" + this.CodeNr + ".GIF");

        //                if (docRenderer != null)
        //                {
        //                    docRenderer.PrepareDocument();
        //                    docRenderer.RenderObject(xGraphics, new PdfSharp.Drawing.XUnit(iX), new PdfSharp.Drawing.XUnit(iY), "24cm", para);
        //                }
        //            }
        //        }
        //    }

        //    //bw.ReportProgress(100);

        //    //if (this.bEnableBarCode)
        //    //{
        //    //    if (docRenderer != null)
        //    //    {
        //    //        if (para != null)
        //    //        {
        //    //            para = sec.AddParagraph();
        //    //            para.AddImage(Environment.GetEnvironmentVariable("TEMP") + "\\BARCODE" + this.CodeNr + ".GIF");

        //    //            if (docRenderer != null)
        //    //            {
        //    //                docRenderer.PrepareDocument();
        //    //                docRenderer.RenderObject(xGraphics, new PdfSharp.Drawing.XUnit(page.Width - 125), new PdfSharp.Drawing.XUnit(0), "12cm", para);
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    if (IsThread)
        //    {
        //        if (xGraphics != null)
        //            xGraphics.Save();

        //        ++Counter;
        //        n++;
        //    }

        //    //if (OnFinish != null)
        //    //    OnFinish();

        //    //this.prog.Close();
        //    //Save("");
        }

        private System.ComponentModel.BackgroundWorker bw = new System.ComponentModel.BackgroundWorker();

        private System.Threading.Thread[] threadPool = new System.Threading.Thread[10000];

        private delegate void PDFCreationDelegate();
        private event PDFCreationDelegate OnFinish;

        private int nThreads = 0;

        [DispId(0)]
        public string Create()
        {
            //OnFinish += new PDFCreationDelegate(CreatePDF_OnFinish);
            
            string s = string.Empty;
            try
            {
                try
                {
                    //Array.Resize<System.Threading.Thread>(ref threadPool, nThreads + 1);
                    //System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CreatePDFThread));
                    
                    //threadPool[nThreads++] = th;
                    //th.Start(null);
                    CreatePDFThread(null);
                    //System.IO.File.Create(@"E:\finish.pdf-file");

                    //bw = new System.ComponentModel.BackgroundWorker();
                    //bw.DoWork += new System.ComponentModel.DoWorkEventHandler(bw_DoWork);
                    //bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                    //bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bw_ProgressChanged);

                    ////prog.Show();

                    //bw.WorkerReportsProgress = true;

                    //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(CreatePDFThread), "TEST");
                    //nThreads++;        
                    //System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(CreatePDFThread));
                    //th.Start();
                    //while (th.IsAlive) { }
                    
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);   
                }
            }
            finally
            {
                s = this.FileName;
            }

            return s;
        }

        private void CreatePDF_OnFinish()
        {
            //Save(string.Empty);
            if (nThreads > -1)
                nThreads--;
        }
        
        void bw_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //prog.pBarProgress.Value = e.ProgressPercentage;
            //prog.pBarProgress.Refresh();
        }

        void bw_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
           

            Save(string.Empty);
            //System.Diagnostics.Process.Start(this.FileName);
            //prog.Hide();
            //prog.pBarProgress.Value = 0;
            //System.IO.File.Create(@"E:\finish.pdf-file");
        }

        void bw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            
            CreatePDFThread(null);
        }

        //void docRenderer_PrepareDocumentProgress(object sender, DocumentRenderer.PrepareDocumentProgressEventArgs e)
        //{
        //    this.prog.lblProz.Text = this.prog.lblProz.Text + " (" + e.Value.ToString() + ")";
        //    this.prog.lblProz.Refresh();

        //    System.Windows.Forms.Application.DoEvents();
        //}

        private static void ReplaceFont(string Name, string Color, int emSize, ref string Text)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                Text = Text.TrimStart().Replace("name='" + Name + "' ", string.Empty);
                Text = Text.TrimStart().Replace("name='" + Name + "'", string.Empty);
            }
            if (!string.IsNullOrEmpty(Color))
            {
                Text = Text.TrimStart().Replace("color='" + Color + "' ", string.Empty);
                Text = Text.TrimStart().Replace("color='" + Color + "'", string.Empty);
            }
            if (!string.IsNullOrEmpty(emSize.ToString()))
            {
                Text = Text.TrimStart().Replace("size='" + emSize.ToString() + "' ", string.Empty);
                Text = Text.TrimStart().Replace("size='" + emSize.ToString() + "'", string.Empty);
            }
        }

        private static void ReplaceTags(ref string Text)
        {
            Text = Text.Replace("</font>", string.Empty);
            Text = Text.Replace("<font ", string.Empty);
            Text = Text.Replace("<b>", string.Empty);
            Text = Text.Replace("</b>", string.Empty);
            Text = Text.Replace(">", string.Empty);
        }


        public string ReplaceColor(string Message, string R, string G, string B)
        {
            string StringWithoutColor = string.Empty;

            StringWithoutColor = Message.Replace("</Color>", string.Empty);
            StringWithoutColor = StringWithoutColor.Replace("<Color=" + R + " " + G + " " + B + ">", string.Empty);

            return StringWithoutColor;
        }
    }
}
