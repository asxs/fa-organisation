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
    public sealed class WorkUnitAddress
        : IWorkUnitDef
    {
        public WorkUnitAddress()
        {

        }

        public long Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public short Plz { get; set; }
        public short Hnr { get; set; }

        public string TableName { get { return "ASXS_ADDRESS"; } set { ; } }

        public string ToSqlString(StatementType type, Units units, long id = 0)
        {
            var commandText =
                string.Empty;

            switch (type)
            {
                case StatementType.Insert:
                    commandText = string.Format("INSERT INTO {0} (id, city, street, plz, hnr) VALUES ({1}, '{2}', '{3}', {4}, {5})", TableName, id, City, Street, Plz, Hnr);
                    break;
            }

            return commandText;
        }
    }
}
