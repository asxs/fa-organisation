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

namespace IxSApp
{

    /// <summary>
    /// 
    /// </summary>
    public class DataLayerProperties
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataLayerProperties"/> class.
        /// </summary>
        public DataLayerProperties()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLayerProperties"/> class.
        /// </summary>
        /// <param name="typeOfEditing">The type of editing.</param>
        public DataLayerProperties(StatementType typeOfEditing)
        {

        }

        /// <summary>
        /// Adds the specified type of editing.
        /// </summary>
        /// <param name="typeOfEditing">The type of editing.</param>
        /// <returns></returns>
        protected virtual IDbCommand Add(StatementType typeOfEditing)
        {
            return default(IDbCommand);
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        /// <returns></returns>
        protected virtual IDbConnection Open()
        {
            return default(IDbConnection);
        }

        /// <summary>
        /// Opens the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        protected virtual IDbConnection Open(string connectionString)
        {
            return default(IDbConnection);
        }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        /// <value>
        /// The select command.
        /// </value>
        public IDbCommand SelectCommand { get; private set; }
        /// <summary>
        /// Gets the update command.
        /// </summary>
        /// <value>
        /// The update command.
        /// </value>
        public IDbCommand UpdateCommand { get; private set; }
        /// <summary>
        /// Gets the insert command.
        /// </summary>
        /// <value>
        /// The insert command.
        /// </value>
        public IDbCommand InsertCommand { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether [use transaction].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use transaction]; otherwise, <c>false</c>.
        /// </value>
        public bool UseTransaction { get; set; }
    }
}
