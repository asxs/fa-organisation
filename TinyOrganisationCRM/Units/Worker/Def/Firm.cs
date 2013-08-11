/* 
 * Builded by Lars Ulrich Herrmann (c) 2013 with f.fN. Sense Applications in year August 2013
 * The code can be used in other Applications if it makes sense
 * If it makes sense the code can be used in this Application
 * I hold the rights on all lines of code and if it makes sense you can contact me over the publish site
 * Feel free to leave a comment
 * 
 * Good Bye
 * 
 */

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

namespace IxSApp
{
    public sealed class WorkUnitFirm
        : IWorkUnitDef
    {
        public WorkUnitFirm()
        {

        }

        public long Id { get; set; }
        public long Id_Bew { get; set; }
        public long Id_Addr { get; set; }
        public long Id_Memo { get; set; }
        public long Id_Mandant { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public bool ReplyRequired { get; set; }

        public string TableName
        {
            get
            {
                return "ASXS_FIRM";
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
                    commandText = string.Format("INSERT INTO {0} (id, id_bew, id_addr, name, id_memo, website, id_man) VALUES ({1}, {2}, {3}, '{4}', {5}, '{6}', {7})", TableName, id, units.Bewerbung.Id, units.Address.Id, Name, units.Memo.Id, Website, Id_Mandant);
                    break;
                case StatementType.Update:

                    commandText = string.Concat("UPDATE ASXS_FIRM SET ID_MAN = " + Id_Mandant + " WHERE ID = " + id) + ";";

                    commandText +=
                        string.Concat("UPDATE V_FIRM SET Firma = '", Name, "', Website = '", Website, "', ID_ADDR = ", Id_Addr, ", ID_MEMO = ", Id_Memo, ", ID_BEW = ", Id_Bew, ", REPLY_REQ = ", ReplyRequired ? 1 : 0, " WHERE ID = ", id);
                    break;
            }

            return commandText;
        }
    }
}
