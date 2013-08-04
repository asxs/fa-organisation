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
    public sealed class AsFirm
        : IDataUnitPackage
    {
        public AsFirm()
        {

        }

        public long Id { get; set; }
        public long Id_Bew { get; set; }
        public long Id_Addr { get; set; }
        public long Id_Memo { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }

        public string TableName
        {
            get
            {
                return "ASXS_FIRM";
            }
            set { ; }
        }

        public string ToSqlString(StatementType type, DataUnitPackage units, long id = 0)
        {
            var commandText =
                string.Empty;

            switch (type)
            {
                case StatementType.Insert:
                    commandText = string.Format("INSERT INTO {0} (id, id_bew, id_addr, name, id_memo, website) VALUES ({1}, {2}, {3}, '{4}', {5}, '{6}')", TableName, id, units.Bewerbung.Id, units.Address.Id, Name, units.Memo.Id, Website);
                    break;
                case StatementType.Update:
                    commandText =
                        string.Concat("UPDATE V_FIRM ", TableName, " SET Firma = '", Name, "', Website = '", Website, "', ID_ADDR = ", Id_Addr, ", ID_MEMO = ", Id_Memo, ", ID_BEW = ", Id_Bew, " WHERE ID = ", id);
                    break;
            }

            return commandText;
        }
    }
}
