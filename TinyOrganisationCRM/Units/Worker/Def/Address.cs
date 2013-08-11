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
    public sealed class WorkUnitAddress
        : IWorkUnitDef
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkUnitAddress"/> class.
        /// </summary>
        public WorkUnitAddress()
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
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        /// <value>
        /// The street.
        /// </value>
        public string Street { get; set; }
        /// <summary>
        /// Gets or sets the PLZ.
        /// </summary>
        /// <value>
        /// The PLZ.
        /// </value>
        public short Plz { get; set; }
        /// <summary>
        /// Gets or sets the HNR.
        /// </summary>
        /// <value>
        /// The HNR.
        /// </value>
        public short Hnr { get; set; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get { return "ASXS_ADDRESS"; } set { ; } }

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
                    commandText = string.Format("INSERT INTO {0} (id, city, street, plz, hnr) VALUES ({1}, '{2}', '{3}', {4}, {5})", TableName, id, City, Street, Plz, Hnr);
                    break;
            }

            return commandText;
        }
    }
}
