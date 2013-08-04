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
    public sealed class NullDataUnit
        : IDataUnit
    {
        public NullDataUnit()
        {

        }

        #region NullDataUnit (IDataUnit)

        public DataUnitPriority Priority { get; set; }

        public IDbCommand Command { get; set; }

        public string Table { get { return ""; } }

        public string Database { get; set; }

        public QueryState Insert(DataUnitPackage units, long id = 0)
        {
            return QueryState.None;
        }

        public QueryState Update(DataUnitPackage units, long id = 0)
        {
            return QueryState.None;
        }

        public QueryState Remove(DataUnitPackage units, long id = 0)
        {
            return QueryState.None;
        }

        #endregion
    }
}
