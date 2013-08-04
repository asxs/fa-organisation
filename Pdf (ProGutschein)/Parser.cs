using System.Collections.Generic;
using System.Xml;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace PrintLayout.PDF
{
    public class HTMLParser
    {
        public struct Lists
        {
            public IList<int> Positions { get; set; }
            public IList<PdfSharp.Drawing.XColor> Color { get; set; }
            public IList<PdfSharp.Drawing.XFont> Font { get; set; }
            public IList<bool> FontStyle { get; set; }
        }

        public class TextStyle
        {
            public bool IsBold { get; set; }
            public string FontFamily { get; set; }
            public double FontSize { get; set; }
            public string Text { get; set; }
            public string FontColor { get; set; }
            public int Position { get; set; }
        }

        private XmlDocument Document;

        public HTMLParser()
        {

        }

        public Lists GetLoadedStyles(IList<HTMLParser.TextStyle> parsingObjects, ref string Text)
        {
            Lists tempLists = new Lists();

            tempLists.Color = new List<PdfSharp.Drawing.XColor>();
            tempLists.Font = new List<PdfSharp.Drawing.XFont>();
            tempLists.FontStyle = new List<bool>();
            tempLists.Positions = new List<int>();

            foreach (HTMLParser.TextStyle htmlTextStyles in parsingObjects)
            {
                tempLists.Positions.Add(htmlTextStyles.Position);
                tempLists.Color.Add(PdfSharp.Drawing.XColor.FromName(htmlTextStyles.FontColor));
                tempLists.Font.Add(new PdfSharp.Drawing.XFont(htmlTextStyles.FontFamily, (htmlTextStyles.FontSize == 0.0 ? 10.0d : htmlTextStyles.FontSize), (htmlTextStyles.IsBold ? PdfSharp.Drawing.XFontStyle.Bold : PdfSharp.Drawing.XFontStyle.Regular)));

                ReplaceFont(htmlTextStyles.FontFamily, htmlTextStyles.FontColor, (int)(htmlTextStyles.FontSize == 0.0 ? 10.0d : htmlTextStyles.FontSize), ref Text);
            }

            ReplaceTags(ref Text);

            return tempLists;
        }

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

        public IList<HTMLParser.TextStyle> Load(string Text)
        {
            IList<TextStyle> lstTemp = new List<TextStyle>();

            Text = "<html>" + Text + "</html>";

            Document = new XmlDocument();
            Document.LoadXml(Text);

            TextStyle item = null;

            int Pos = 0;
            foreach (XmlNode node in Document.ChildNodes[0].ChildNodes)
            {
                item = new TextStyle();
                Parse(Text, node, ref item);

                if (item != null)
                {
                    Pos += item.Position;
                    item.Position = Pos;
                }

                lstTemp.Add(item);
            }

            return lstTemp;
        }

        public void Parse(string Text, XmlNode node, ref TextStyle ts)
        {
            switch (node.Name.ToUpper())
            {
                case "FONT":
                    foreach (XmlAttribute attribute in node.Attributes)
                    {
                        switch (attribute.Name.ToUpper())
                        {
                            case "NAME":
                                ts.FontFamily = attribute.Value;
                                break;
                            case "SIZE":
                                ts.FontSize = double.Parse(attribute.Value);
                                break;
                            case "COLOR":
                                ts.FontColor = attribute.Value;
                                break;
                        }
                    }
                    break;
                case "B":
                    ts.IsBold = true;
                    break;
                case "#TEXT":
                    ts.Text = node.Value;
                    ts.Position = ts.Position + node.Value.Length;
                    break;
            }

            if (node.HasChildNodes)
            {
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    Parse(Text, childNode, ref ts);
                }
            }
        }
    }

    public class PDF
    {
        private PdfSharp.Pdf.PdfDocument Document;
        private PdfSharp.Pdf.PdfPage Page;
        private PdfSharp.Drawing.XGraphics Graphics;

        public PDF(PdfSharp.Pdf.PdfDocument Document, PdfSharp.Pdf.PdfPage Page, PdfSharp.Drawing.XGraphics Graphics)
        {
            this.Document = Document;
            this.Page = Page;
            this.Graphics = Graphics;
        }

        private IList<string> SplitAtPosition(string Text, IList<int> Positions)
        {
            IList<string> SeperateStrings = new List<string>();

            int StartPos = 0;

            for (int j = 0; j < Positions.Count; ++j)
            {
                for (int i = 0; i < Text.Length; ++i)
                {
                    if (i == Positions[j])
                    {
                        string SeperatedString = Text.Substring(StartPos, Positions[j] - StartPos);
                        SeperateStrings.Add(SeperatedString);
                        StartPos = i;
                    }
                }
            }

            SeperateStrings.Add(Text.Substring(StartPos));

            return SeperateStrings;
        }

        public void DrawString(IList<PdfSharp.Drawing.XColor> Colors, IList<int> Positions, IList<PdfSharp.Drawing.XFont> Fonts, string Text, int X, int Y, ref Paragraph paragraph, ref DocumentRenderer documentRenderer)
        {
            PdfSharp.Drawing.Layout.XTextFormatter xText = new PdfSharp.Drawing.Layout.XTextFormatter(Graphics);
            IList<string> SeperateStrings = new List<string>();

            SeperateStrings = this.SplitAtPosition(Text, Positions);

            int X2 = X;
            int Y2 = Y;

            IList<int> XContainer = new List<int>();
            IList<string> StringContainer = new List<string>();

            for (int i = 0; i < Colors.Count; ++i)
            {
                StringContainer.Add(SeperateStrings[i]);
                PdfSharp.Drawing.XColor Color = Colors[i];

                MigraDoc.DocumentObjectModel.Font fontt = new MigraDoc.DocumentObjectModel.Font("Arial");
                fontt.Color = new MigraDoc.DocumentObjectModel.Color((uint)Colors[i].ToGdiColor().ToArgb());
                fontt.Name = Fonts[i].Name;
                fontt.Size = Fonts[i].Size;
                fontt.Bold = Fonts[i].Bold;

                
                paragraph.AddFormattedText(SeperateStrings[i].Replace("#", "\n").Replace("\\T#", "\t"), fontt);

                int Pos = Positions[i];
                //if (i > 0)
                //{
                //    PdfSharp.Drawing.XSize Size = Graphics.MeasureString(SeperateStrings[i - 1], new PdfSharp.Drawing.XFont(Fonts[i - 1].Name, Fonts[i - 1].Size, PdfSharp.Drawing.XFontStyle.Regular));
                //    if (!SeperateStrings[i-1].Contains("\n"))
                //    {
                //        X2 += (int)Size.Width;
                //        if (SeperateStrings[i].Contains("\n"))
                //            Y2 += (int)Size.Height;
                //        else
                //        {
                //            Y2 = Y;
                //            //Y2 += (int)Size.Height;
                //        }
                //    }
                //}

                XContainer.Add(X2);

                //Graphics.DrawString(SeperateStrings[i], new PdfSharp.Drawing.XFont(Fonts[i].Name, Fonts[i].Size, PdfSharp.Drawing.XFontStyle.Regular), new PdfSharp.Drawing.XSolidBrush(Color),
                //    new PdfSharp.Drawing.XPoint(X2, Y));

                //    PdfSharp.Drawing.XSize size = Graphics.MeasureString("\n", new PdfSharp.Drawing.XFont("Arial", 10.0));
                //    if (SeperateStrings[i].Contains("\n"))
                //    {
                //        string[] a = SeperateStrings[i].Split('\n');

                //        int k = 0;
                //        foreach (string s in a)
                //        {
                //            if (string.IsNullOrEmpty(s))
                //            {
                //                Y2 += (int)size.Height;
                //                //xText.DrawString(s, new PdfSharp.Drawing.XFont(Fonts[i].Name, Fonts[i].Size, PdfSharp.Drawing.XFontStyle.Regular), new PdfSharp.Drawing.XSolidBrush(Color),
                //                //    new PdfSharp.Drawing.XRect(XContainer[0], Y2, Page.Width, Page.Height));
                //            }
                //            else
                //            {
                //                if (k > 0)
                //                {
                //                    //xText.DrawString(s, new PdfSharp.Drawing.XFont(Fonts[i].Name, Fonts[i].Size, PdfSharp.Drawing.XFontStyle.Regular), new PdfSharp.Drawing.XSolidBrush(Color),
                //                    //new PdfSharp.Drawing.XRect(XContainer[0], Y2, Page.Width, Page.Height));
                //                    if (!s.Contains("\n"))
                //                        Y2 += (int)size.Height;
                //                }
                //                else
                //                {
                //                    string ss = string.Empty;
                //                    for (int m = 0; m < StringContainer.Count - 1; ++m)
                //                        ss += StringContainer[m];

                //                    //xText.DrawString(s, new PdfSharp.Drawing.XFont(Fonts[i].Name, Fonts[i].Size, PdfSharp.Drawing.XFontStyle.Regular), new PdfSharp.Drawing.XSolidBrush(Color),
                //                    //    new PdfSharp.Drawing.XRect(XContainer[XContainer.Count-1], Y, Page.Width, Page.Height));
                //                }
                //            }
                //            k++;
                //        }
                //    }
                //    else
                //    {
                //        //if (!(SeperateStrings.Count-1 == i))
                //        //    xText.DrawString(SeperateStrings[i], new PdfSharp.Drawing.XFont(Fonts[i].Name, Fonts[i].Size, PdfSharp.Drawing.XFontStyle.Regular), new PdfSharp.Drawing.XSolidBrush(Color),
                //        //        new PdfSharp.Drawing.XRect(XContainer[XContainer.Count-1], Y2, Page.Width, Page.Height));
                //        //else
                //        //    xText.DrawString(SeperateStrings[i], new PdfSharp.Drawing.XFont(Fonts[i].Name, Fonts[i].Size, PdfSharp.Drawing.XFontStyle.Regular), new PdfSharp.Drawing.XSolidBrush(Color),
                //        //        new PdfSharp.Drawing.XRect(XContainer[0], Y2, Page.Width, Page.Height));
                //    }
                //}

                //if (SeperateStrings.Count > Colors.Count)
                //{
                //    Text = string.Empty;
                //    for (int i = 0; i < Colors.Count; ++i)
                //        Text += SeperateStrings[i];

                //    PdfSharp.Drawing.XSize Size = Graphics.MeasureString(Text, new PdfSharp.Drawing.XFont("Arial", 10.0D));

                //    X2 = (int)Size.Width;

                //    Graphics.DrawString(SeperateStrings[SeperateStrings.Count-1], new PdfSharp.Drawing.XFont("Arial", 10.0D, PdfSharp.Drawing.XFontStyle.Regular), PdfSharp.Drawing.XBrushes.Black,
                //        new PdfSharp.Drawing.XPoint(X2, Y));
                //}
            }
        }

        public void DrawString(IList<PdfSharp.Drawing.XColor> Colors, IList<int> Positions, IList<PdfSharp.Drawing.XFont> Fonts, string Text, int X, int Y)
        {
            PdfSharp.Drawing.Layout.XTextFormatter xText = new PdfSharp.Drawing.Layout.XTextFormatter(Graphics);

            IList<string> SeperateStrings = new List<string>();

            SeperateStrings = this.SplitAtPosition(Text, Positions);

            int X2 = X;

            for (int i = 0; i < Colors.Count; ++i)
            {
                PdfSharp.Drawing.XColor Color = Colors[i];

                int Pos = Positions[i];
                if (i > 0)
                {
                    PdfSharp.Drawing.XSize Size = Graphics.MeasureString(SeperateStrings[i - 1], new PdfSharp.Drawing.XFont(Fonts[i - 1].Name, Fonts[i - 1].Size, PdfSharp.Drawing.XFontStyle.Regular));
                    X2 += (int)Size.Width;
                }

                xText.Alignment = PdfSharp.Drawing.Layout.XParagraphAlignment.Left;

                PdfSharp.Drawing.XStringFormat xStringFormat = new PdfSharp.Drawing.XStringFormat();

                xText.DrawString(SeperateStrings[i], new PdfSharp.Drawing.XFont(Fonts[i].Name, Fonts[i].Size, PdfSharp.Drawing.XFontStyle.Regular), new PdfSharp.Drawing.XSolidBrush(Color),
                    new PdfSharp.Drawing.XRect(X2, Y, Page.Width, Page.Height));

                //Graphics.DrawString(SeperateStrings[i], new PdfSharp.Drawing.XFont(Fonts[i].Name, Fonts[i].Size, PdfSharp.Drawing.XFontStyle.Regular), new PdfSharp.Drawing.XSolidBrush(Color),
                //    new PdfSharp.Drawing.XPoint(X2, Y));
            }

            if (SeperateStrings.Count > Colors.Count)
            {
                Text = string.Empty;
                for (int i = 0; i < Colors.Count; ++i)
                    Text += SeperateStrings[i];

                PdfSharp.Drawing.XSize Size = Graphics.MeasureString(Text, new PdfSharp.Drawing.XFont("Arial", 10.0D));

                X2 = (int)Size.Width;

                xText.DrawString(SeperateStrings[SeperateStrings.Count - 1], new PdfSharp.Drawing.XFont("Arial", 10.0D, PdfSharp.Drawing.XFontStyle.Regular), PdfSharp.Drawing.XBrushes.Black,
                    new PdfSharp.Drawing.XRect(X2, Y, Page.Width, Page.Height));

                //Graphics.DrawString(SeperateStrings[SeperateStrings.Count - 1], new PdfSharp.Drawing.XFont("Arial", 10.0D, PdfSharp.Drawing.XFontStyle.Regular), PdfSharp.Drawing.XBrushes.Black,
                //    new PdfSharp.Drawing.XPoint(X2, Y));
            }
        }
    }
}
