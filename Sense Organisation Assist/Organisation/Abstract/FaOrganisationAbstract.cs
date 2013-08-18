﻿/* 
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
    public abstract class FaOrganisationAbstract
        : IFaOrganisation, IDisposable
    {
        /// <summary>
        /// The connection
        /// </summary>
        protected SAConnection connection = null;
        /// <summary>
        /// The table id
        /// </summary>
        protected TableIdCommand tableId = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="FaOrganisationAbstract"/> class.
        /// </summary>
        public FaOrganisationAbstract()
        {
            
        }

        #region FaOrganisationAppend (IDisposable)

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                connection = null;
                tableId = null;
            }
        }

        #endregion

        /// <summary>
        /// Opens the internal.
        /// </summary>
        /// <param name="encapsulateWithTransaction">if set to <c>true</c> [encapsulate with transaction].</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">
        /// connection have to be initialized
        /// or
        /// connection have to be open
        /// </exception>
        protected virtual IDbConnection OpenInternal(bool encapsulateWithTransaction)
        {
            if (connection == null)
            {
                connection =
                    ConnectionFactory.CreateAnySystemConnection(ConnectionStringManager.ConnectionStringNetworkServer) as SAConnection;

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

        /// <summary>
        /// Appends the specified package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="unit">The unit.</param>
        /// <param name="saveWithNewId">if set to <c>true</c> [save with new id].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">unit</exception>
        /// <exception cref="System.InvalidOperationException">tableIdCommand have to be initialized</exception>
        public virtual long Append(Units package, IWorkDataUnit unit, bool saveWithNewId = true)
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
                    tableId.Get(saveWithNewId ? "NEW" : string.Empty, unit.Table);

                unit.Command = command;
                unit.Database = "asxs";
                unit.Priority = unit.Table.ToUpper().Contains("firm") ? UnitPriorityType.Head : UnitPriorityType.Children;
                unit.Insert(package, key);
            }

            return key;
        }

        /// <summary>
        /// Edits the specified package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="unit">The unit.</param>
        /// <param name="id">The id.</param>
        /// <exception cref="System.ArgumentNullException">unit</exception>
        /// <exception cref="System.InvalidOperationException">tableIdCommand have to be initialized</exception>
        public virtual void Edit(Units package, IWorkDataUnit unit, long id = 0)
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
                unit.Priority = unit.Table.Contains("firm") ? UnitPriorityType.Head : UnitPriorityType.Children;
                unit.Update(package, key);
            }
        }

        /// <summary>
        /// Rollbacks this instance.
        /// </summary>
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

        /// <summary>
        /// Closes this instance.
        /// </summary>
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
