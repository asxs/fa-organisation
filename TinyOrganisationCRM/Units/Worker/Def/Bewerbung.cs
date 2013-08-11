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
    public sealed class WorkUnitBewerbung
        : IWorkUnitDef
    {
        public WorkUnitBewerbung()
        {

        }

        public long Id { get; set; }
        public bool State { get; set; }
        public bool Sent { get; set; }
        public bool NegativeStateAtOwn { get; set; }
        public bool Reply { get; set; }
        public SqlDateTime Day { get; set; }

        public string TableName { get { return "ASXS_BEWERBUNG"; } set { ; } }

        public string ToSqlString(StatementType type, Units units, long id = 0)
        {
            var commandText =
                string.Empty;

            switch (type)
            {
                case StatementType.Insert:
                    commandText = string.Format("INSERT INTO {0} (id, state, sent, state_own, reply, day) VALUES ({1}, {2}, {3}, {4}, {5}, '{6}')", TableName, id, State ? 1 : 0, Sent ? 1 : 0, NegativeStateAtOwn ? 1 : 0, Reply ? 1 : 0, Day.ToTimeStamp());
                    break;
                case StatementType.Update:
                    commandText = string.Concat("UPDATE V_FIRM SET Absage = ", State ? 1 : 0, ", Abgeschickt = ", Sent ? 1 : 0, ", Tag = '", Day.ToTimeStamp(), "', Rueckmeldung = ", Reply ? 1 : 0, " WHERE ID = ", id);
                    break;
            }

            return commandText;
        }
    }
}
