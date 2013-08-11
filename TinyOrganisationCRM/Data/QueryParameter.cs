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

namespace As.Data
{
    public struct QueryParameter
    {
        public IDbCommand Command;
        public string CommandText;
        public bool Compile;
        public bool MakeAsync;
    }
}
