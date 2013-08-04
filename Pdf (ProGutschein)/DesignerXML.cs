using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace PrintLayout.Designer_XML
{
    public class DesignerXMLControls
    {
        public DesignerXMLControls()
        {

        }

        public struct Location
        {
            public int X;
            public int Y;
        }

        public struct Size
        {
            public int Width;
            public int Height;
        }

        public Location ControlLocation { get; set; }

        public Size ControlSize { get; set; }

        public string Text { get; set; }

        public System.Drawing.Image Image { get; set; }

        public string Section { get; set; }
    }
}
