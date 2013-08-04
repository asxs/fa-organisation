using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace As
{
    public sealed class DataPackage
    {
        public DataPackage()
        {

        }

        public long Id { get; set; }
        public string TableName { get; set; }

        public DataUnitPackage Item { get; set; }
    }
}
