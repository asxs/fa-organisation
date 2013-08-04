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

namespace As
{
    public abstract class FaOrganisationAbstract
        : IFaOrganisation, IDisposable
    {
        protected SAConnection connection = null;
        protected TableIdCommand tableId = null;

        public FaOrganisationAbstract()
        {
            
        }

        #region FaOrganisationAppend (IDisposable)

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                connection = null;
                tableId = null;
            }
        }

        #endregion

        protected virtual IDbConnection OpenInternal(bool encapsulateWithTransaction)
        {
            if (connection == null)
            {
                connection =
                    ASxDataFactory.CreateAnySystemConnection(ConnectionStringManager.TinyOrganisationCrmAnyConnectionString) as SAConnection;

                connection.Open();

                if (connection == null)
                    throw new InvalidOperationException("connection have to be initialized");

                if (connection.State != ConnectionState.Open)
                    throw new InvalidOperationException("connection have to be open");

                if (encapsulateWithTransaction)
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "begin transaction";
                        command.Prepare();
                        command.ExecuteNonQuery();
                    }
            }

            return connection;
        }

        public virtual long Append(DataUnitPackage package, IDataUnit unit)
        {
            var key = 0L;

            if (unit == null)
                throw new ArgumentNullException("unit");

            OpenInternal(encapsulateWithTransaction: true);

            using (var command =
                connection.CreateCommand())
            {
                var tableId = 
                    TableIdCommand.CreateInstance(command as IDbCommand);

                if (tableId == null)
                    throw new InvalidOperationException("tableIdCommand have to be initialized");

                key =
                    tableId.Get("NEW", unit.Table);

                unit.Command = command;
                unit.Database = "asxs";
                unit.Priority = unit.Table.ToUpper().Contains("firm") ? DataUnitPriority.Head : DataUnitPriority.Children;
                unit.Insert(package, key);
            }

            return key;
        }

        public virtual void Edit(DataUnitPackage package, IDataUnit unit, long id = 0)
        {
            var key = id;

            if (unit == null)
                throw new ArgumentNullException("unit");

            OpenInternal(encapsulateWithTransaction: true);

            using (var command =
                connection.CreateCommand())
            {
                tableId =
                    TableIdCommand.CreateInstance(command);

                if (tableId == null)
                    throw new InvalidOperationException("tableIdCommand have to be initialized");

                unit.Command = command;
                unit.Database = "asxs";
                unit.Priority = unit.Table.Contains("firm") ? DataUnitPriority.Head : DataUnitPriority.Children;
                unit.Update(package, key);
            }
        }

        protected virtual void Rollback()
        {
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "rollback";
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
            catch { }
        }

        public virtual void Close()
        {
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "commit";
                    command.Prepare();
                    command.ExecuteNonQuery();
                }
            }
            catch { }
        }
    }

}
