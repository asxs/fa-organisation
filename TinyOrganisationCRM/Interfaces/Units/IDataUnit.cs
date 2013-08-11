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
    public interface IWorkDataUnit
    {
        UnitPriorityType Priority { get; set; }
        IDbCommand Command { get; set; }

        string Table { get; }

        string Database { get; set; }

        QueryStateType Insert(Units units, long id = 0);

        QueryStateType Update(Units units, long id = 0);

        QueryStateType Remove(Units units, long id = 0);

    }
}
