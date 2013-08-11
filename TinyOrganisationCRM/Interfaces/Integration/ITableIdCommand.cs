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
    /// <summary>
    /// 
    /// </summary>
    public sealed class TableIdCommand : ITableId
    {
        /// <summary>
        /// The command
        /// </summary>
        private IDbCommand command =
            null;
        /// <summary>
        /// The single
        /// </summary>
        internal static TableIdCommand single =
            null;

        /// <summary>
        /// Prevents a default instance of the <see cref="TableIdCommand"/> class from being created.
        /// </summary>
        TableIdCommand()
        {

        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">command</exception>
        /// <exception cref="System.InvalidOperationException">connection have to be initialized</exception>
        public static TableIdCommand CreateInstance(IDbCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            if (command.Connection.State != ConnectionState.Open)
                throw new InvalidOperationException("connection have to be initialized");

            if (single != null && single.command == null)
                single.command = command;

            if (single == null)
                single = new TableIdCommand();

            if (single.command == null || single.command.Connection == null)
                single.command = command;

            return
                single;
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        /// <exception cref="System.InvalidOperationException">Call CreateInstance before</exception>
        public static TableIdCommand Instance
        {
            get
            {
                if (single == null)
                    throw new InvalidOperationException("Call CreateInstance before");

                return single;
            }
        }

        #region DataTableIdCommand (ITableId)

        /// <summary>
        /// Gets the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">tableName</exception>
        /// <exception cref="System.InvalidOperationException">id must be higher then zero</exception>
        public long Get(string type, string tableName)
        {
            type = type.ToUpper();

            if (tableName == string.Empty || tableName == null)
                throw new ArgumentNullException("tableName");

            var id = 1L;

            try
            {
                if (!(TableExist(tableName)))
                    InsertTableWithDefaultId(tableName);
                
                command.CommandText = string.Concat("SELECT TABLE_ID FROM ASXS_IDS WHERE TABLE_NAME = '", tableName, "'");
                command.Prepare();

                try
                {
                    id = (long)command.ExecuteScalar();

                    switch (type)
                    {
                        case "NEW":
                            {
                                id = ThrowNewIdInternal(tableName, id);

                                if (id == 0)
                                    throw new InvalidOperationException("id must be higher then zero");

                                command.CommandText =
                                    string.Concat("UPDATE ASXS_IDS SET TABLE_ID = ", id, " WHERE TABLE_NAME = '", tableName.ToUpper(), "'");
                                try
                                {
                                    command.Prepare();
                                    command.ExecuteNonQuery();
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
                            }
                            break;
                        default:
                            {
                                //Nothing todo in this case (Default)
                            }
                            break;
                    }
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
            }
            catch (InvalidOperationException ex)
            {

            }
            catch (Exception ex)
            {

            }

            return id;
        }

        /// <summary>
        /// Throws the new id internal.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">id must be higher then zero</exception>
        internal long ThrowNewIdInternal(string tableName, long id)
        {
            if (id == -1)
                throw new ArgumentException("id must be higher then zero");

            var key = IncrIdWithOne(id);
            return key;
        }

        /// <summary>
        /// Inserts the table with default id.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <exception cref="System.ArgumentNullException">tableName</exception>
        private void InsertTableWithDefaultId(string tableName)
        {
            if (tableName == string.Empty || tableName == null)
                throw new ArgumentNullException("tableName");

            command.CommandText = string.Concat("INSERT INTO ASXS_IDS (TABLE_NAME, TABLE_ID) VALUES ('", tableName, "',", 1, ")");
            try
            {
                command.Prepare();
                command.ExecuteNonQuery();
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
        }

        /// <summary>
        /// Tables the exist.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">tableName</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:SQL-Abfragen auf Sicherheitsrisiken überprüfen")]
        private bool TableExist(string tableName)
        {
            if (tableName == string.Empty || tableName == null)
                throw new ArgumentNullException("tableName");

            command.CommandText = string.Concat("SELECT * FROM ASXS_IDS WHERE TABLE_NAME = '", tableName, "'");

            var tableIsAlive = false;
            try
            {
                command.Prepare();

                var reader
                    = command.ExecuteReader() as SADataReader;

                tableIsAlive = reader.HasRows;
                try
                {
                    reader.Close();
                }
                catch { }
                finally
                {
                    reader.Dispose();
                    reader = null;
                }
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

            return tableIsAlive;
        }

        /// <summary>
        /// Incrs the id with one.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        private static long IncrIdWithOne(long id)
        {
            var key = id + 1;
            return key;
        }

        /// <summary>
        /// Decrs the id with one.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        private static long DecrIdWithOne(long id)
        {
            var key = id - 1;
            return key;
        }

        #endregion
    }

}
