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
    public sealed class WorkUnitMandant
    : IWorkUnitDef
    {
        public WorkUnitMandant()
        {

        }

        public long Id { get; set; }
        public bool ReplyRequired { get; set; }

        public string TableName
        {
            get
            {
                return "ASXS_MANDANT";
            }
            set { ; }
        }

        public string ToSqlString(StatementType type, Units units, long id = 0)
        {
            var commandText =
                string.Empty;

            switch (type)
            {
                case StatementType.Insert:
                    commandText = string.Concat("INSERT INTO ASXS_MANDANT (ID, ID_FIRM, REPLY_REQ) VALUES (", id, ",", ReplyRequired ? 1 : 0, ")");
                    break;
                case StatementType.Update:
                    break;
            }

            return commandText;
        }
    }
}
