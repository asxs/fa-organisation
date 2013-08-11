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
    public sealed class WorkUnitMandant
    : IWorkUnitDef
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkUnitMandant"/> class.
        /// </summary>
        public WorkUnitMandant()
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
                return "ASXS_MANDANT";
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
                    commandText = string.Concat("INSERT INTO ASXS_MANDANT (ID, ID_FIRM, REPLY_REQ) VALUES (", id, ",", ReplyRequired ? 1 : 0, ")");
                    break;
                case StatementType.Update:
                    break;
            }

            return commandText;
        }
    }
}
