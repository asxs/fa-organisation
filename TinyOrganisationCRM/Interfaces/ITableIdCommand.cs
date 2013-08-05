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
    public sealed class TableIdCommand : ITableId
    {
        private IDbCommand command =
            null;
        internal static TableIdCommand single =
            null;

        TableIdCommand()
        {

        }

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

        internal long ThrowNewIdInternal(string tableName, long id)
        {
            if (id == -1)
                throw new ArgumentException("id must be higher then zero");

            var key = IncrIdWithOne(id);
            return key;
        }

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

        private static long IncrIdWithOne(long id)
        {
            var key = id + 1;
            return key;
        }

        private static long DecrIdWithOne(long id)
        {
            var key = id - 1;
            return key;
        }

        #endregion
    }

}
