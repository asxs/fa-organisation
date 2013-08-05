using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
{
    public struct DataUnitPackage
    {
        public AsBewerbung Bewerbung { get; set; }
        public AsMemoPackage Memo { get; set; }
        public AsAddress Address { get; set; }
        public AsFirm Firm { get; set; }
        public AsAnlage Anlage { get; set; }
    }
}
