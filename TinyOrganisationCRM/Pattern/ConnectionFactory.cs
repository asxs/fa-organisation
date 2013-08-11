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

namespace As.Data
{
    public static class ConnectionFactory
    {
        public static IDbConnection CreateAnySystemConnection(string connectionString)
        {
            if (connectionString == string.Empty || connectionString == null)
                throw new ArgumentNullException("connectionString");

            return new SAConnection(connectionString: connectionString);
        }
    }
}
