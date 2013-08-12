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
    public sealed class WorkUnitBewerbung
        : IWorkUnitDef
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkUnitBewerbung"/> class.
        /// </summary>
        public WorkUnitBewerbung()
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
        /// Gets or sets a value indicating whether this <see cref="WorkUnitBewerbung"/> is state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if state; otherwise, <c>false</c>.
        /// </value>
        public bool State { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WorkUnitBewerbung"/> is sent.
        /// </summary>
        /// <value>
        ///   <c>true</c> if sent; otherwise, <c>false</c>.
        /// </value>
        public bool Sent { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [negative state at own].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [negative state at own]; otherwise, <c>false</c>.
        /// </value>
        public bool NegativeStateAtOwn { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WorkUnitBewerbung"/> is reply.
        /// </summary>
        /// <value>
        ///   <c>true</c> if reply; otherwise, <c>false</c>.
        /// </value>
        public bool Reply { get; set; }
        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        /// <value>
        /// The day.
        /// </value>
        public SqlDateTime Day { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WorkUnitBewerbung"/> is zusage.
        /// </summary>
        /// <value>
        ///   <c>true</c> if zusage; otherwise, <c>false</c>.
        /// </value>
        public bool Zusage { get; set; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string TableName { get { return "ASXS_BEWERBUNG"; } set { ; } }

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
                    commandText = string.Format("INSERT INTO {0} (id, state, sent, state_own, reply, day, positive_reply) VALUES ({1}, {2}, {3}, {4}, {5}, '{6}', {7})", TableName, id, State ? 1 : 0, Sent ? 1 : 0, NegativeStateAtOwn ? 1 : 0, Reply ? 1 : 0, Day.ToTimeStamp(), Zusage ? 1 : 0);
                    break;
                case StatementType.Update:
                    commandText = string.Concat("UPDATE V_FIRM SET Zusage = ", Zusage ? 1 : 0, ", Absage = ", State ? 1 : 0, ", Abgeschickt = ", Sent ? 1 : 0, ", Tag = '", Day.ToTimeStamp(), "', Rueckmeldung = ", Reply ? 1 : 0, " WHERE ID = ", id);
                    break;
            }

            return commandText;
        }
    }
}
