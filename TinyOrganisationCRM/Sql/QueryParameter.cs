using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;

#region iAnywhere.Data.SQLAnywhere.v3.5

using iAnywhere;
using iAnywhere.Data;
using iAnywhere.Data.SQLAnywhere;

#endregion

namespace As
{
    public struct QueryParameter
    {
        public IDbCommand Command { get; set; }
        public string CommandText { get; set; }
        public bool Compile { get; set; }
        public bool MakeAsync { get; set; }
    }
}
