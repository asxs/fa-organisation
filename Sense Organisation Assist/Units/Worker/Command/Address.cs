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
    using Data;

    /// <summary>
    /// 
    /// </summary>
    public sealed class AddressDataUnit : IWorkDataUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressDataUnit"/> class.
        /// </summary>
        public AddressDataUnit()
        {

        }

        #region AddressDataUnit (IDataUnit)

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public UnitPriorityType Priority { get; set; }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        public IDbCommand Command { get; set; }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>
        /// The table.
        /// </value>
        public string Table { get { return "ASXS_ADDRESS"; } }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public string Database { get; set; }

        /// <summary>
        /// Inserts the specified units.
        /// </summary>
        /// <param name="units">The units.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public QueryStateType Insert(Units units, long id = 0)
        {
            var state = QueryStateType.None;
            try
            {
                state = SqlQuery.Insert
                (
                    new QueryParameter()
                    {
                        Command = Command,
                        CommandText = units.Address.ToSqlString(StatementType.Insert, units, id),
                        Compile = true,
                        MakeAsync = false
                    }
                );
            }
            catch (SAException ex)
            {

            }
            catch (InvalidOperationException ex)
            {

            }
            catch (Exception ex)
            {

            }

            return state;
        }

        /// <summary>
        /// Updates the specified units.
        /// </summary>
        /// <param name="units">The units.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public QueryStateType Update(Units units, long id = 0)
        {
            var state = QueryStateType.None;
            try
            {
                state = SqlQuery.Update
                (
                    new QueryParameter()
                    {
                        Command = Command,
                        CommandText = units.Address.ToSqlString(StatementType.Update, units, id),
                        Compile = true,
                        MakeAsync = false
                    }
                );
            }
            catch (SAException ex)
            {

            }
            catch (InvalidOperationException ex)
            {

            }
            catch (Exception ex)
            {

            }

            return state;
        }

        /// <summary>
        /// Removes the specified units.
        /// </summary>
        /// <param name="units">The units.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public QueryStateType Remove(Units units, long id = 0)
        {
            return QueryStateType.None;
        }

        #endregion
    }
}
