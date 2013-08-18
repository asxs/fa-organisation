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

namespace IxSApp.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConnectionFactory
    {
        /// <summary>
        /// Creates any system connection.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">connectionString</exception>
        public static IDbConnection CreateAnySystemConnection(string connectionString)
        {
            if (connectionString == string.Empty || connectionString == null)
                throw new ArgumentNullException("connectionString");

            return new SAConnection(connectionString: connectionString);
        }
    }
}
