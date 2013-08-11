﻿using System;
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
    using Data;

    public sealed class NullDataUnit
        : IWorkDataUnit
    {
        public NullDataUnit()
        {

        }

        #region NullDataUnit (IDataUnit)

        public UnitPriorityType Priority { get; set; }

        public IDbCommand Command { get; set; }

        public string Table { get { return ""; } }

        public string Database { get; set; }

        public QueryStateType Insert(Units units, long id = 0)
        {
            return QueryStateType.None;
        }

        public QueryStateType Update(Units units, long id = 0)
        {
            return QueryStateType.None;
        }

        public QueryStateType Remove(Units units, long id = 0)
        {
            return QueryStateType.None;
        }

        #endregion
    }
}
