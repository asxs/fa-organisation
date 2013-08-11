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
    public sealed class WorkUnitAnlage
        : IWorkUnitDef
    {
        public WorkUnitAnlage()
        {

        }

        public List<AnlageItemInfo> Anlagen { get; set; }

        public string TableName
        {
            get
            {
                return "ASXS_ANLAGE";
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
                    break;
                case StatementType.Update:
                    break;
            }

            return commandText;
        }
    }
}
