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
    /// <summary>
    /// 
    /// </summary>
    public sealed class WorkUnitFirm
        : IWorkUnitDef
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkUnitFirm"/> class.
        /// </summary>
        public WorkUnitFirm()
        {

        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public long Id { get; set; }
        /// <summary>
        /// Gets or sets the id_ bew.
        /// </summary>
        /// <value>
        /// The id_ bew.
        /// </value>
        public long Id_Bew { get; set; }
        /// <summary>
        /// Gets or sets the id_ addr.
        /// </summary>
        /// <value>
        /// The id_ addr.
        /// </value>
        public long Id_Addr { get; set; }
        /// <summary>
        /// Gets or sets the id_ memo.
        /// </summary>
        /// <value>
        /// The id_ memo.
        /// </value>
        public long Id_Memo { get; set; }
        /// <summary>
        /// Gets or sets the id_ mandant.
        /// </summary>
        /// <value>
        /// The id_ mandant.
        /// </value>
        public long Id_Mandant { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        /// <value>
        /// The website.
        /// </value>
        public string Website { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [reply required].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [reply required]; otherwise, <c>false</c>.
        /// </value>
        public bool ReplyRequired { get; set; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName
        {
            get
            {
                return "ASXS_FIRM";
            }
            set { ; }
        }

        /// <summary>
        /// To the SQL string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="units">The units.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
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
