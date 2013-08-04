﻿using System;
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

using System.Threading;

namespace As
{
    public static class SqlQuery
    {
        public static QueryState Insert(QueryParameter parameter)
        {
            if (((object)parameter == null))
                throw new ArgumentNullException("parameter");

            var command = parameter.Command;
            var commandText =
                parameter.CommandText;

            if (commandText == string.Empty || commandText == null)
                throw new ArgumentNullException("commandText");

            //like a method-contract
            if (commandText.Substring(0, "INSERT".Length).ToUpper() != "INSERT")
                throw new InvalidOperationException("Doesn't contain the correct DDL command");

            if (commandText.Substring(0, "INSERT INTO".Length).ToUpper() != "INSERT INTO" || !commandText.Contains("VALUES"))
                throw new InvalidOperationException("Syntax problem in correct DDL command");

            if (command == null)
                throw new NullReferenceException("command");

            if (command.Connection.State != ConnectionState.Open)
                throw new InvalidOperationException("Connection must be opened before");

            command.CommandText = commandText;

            if (parameter.Compile)
                command.Prepare();

            switch (parameter.MakeAsync)
            {
                case true:
                    {
                        var anyCommand =
                            parameter.Command as SACommand;

                        if (anyCommand == null)
                            throw new InvalidOperationException("anyCommand have to be converted in SACommand, if the Async - Method is used");

                        try
                        {
                            var asyncResult =
                                anyCommand.BeginExecuteNonQuery();

                            while (!(asyncResult.IsCompleted))
                                Thread.Sleep(new TimeSpan(5)); //five nanoseconds

                            anyCommand.EndExecuteNonQuery(asyncResult);
                        }
                        catch (SAException ex) //BeginExecuteQuery
                        {
                            throw ex;
                        }
                        catch (ArgumentException ex) //EndExecuteNonQuery
                        {
                            throw ex;
                        }
                        catch (InvalidOperationException ex) //EndExecuteNonQuery
                        {
                            throw ex;
                        }
                    }
                    break;
                default:
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SAException ex)
                    {
                        throw ex;
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    break;
            }

            return QueryState.None;
        }

        public static void Select()
        {

        }

        public static QueryState Update(QueryParameter parameter)
        {
            if (((object)parameter == null))
                throw new ArgumentNullException("parameter");

            var command = parameter.Command;
            var commandText =
                parameter.CommandText;

            if (commandText == string.Empty || commandText == null)
                throw new ArgumentNullException("commandText");

            //like a method-contract
            if (commandText.Substring(0, "UPDATE".Length).ToUpper() != "UPDATE")
                throw new InvalidOperationException("Doesn't contain the correct DDL command");

            if (command == null)
                throw new NullReferenceException("command");

            if (command.Connection.State != ConnectionState.Open)
                throw new InvalidOperationException("Connection must be opened before");

            command.CommandText = commandText;

            if (parameter.Compile)
                command.Prepare();

            switch (parameter.MakeAsync)
            {
                case true:
                    {
                        var anyCommand =
                            parameter.Command as SACommand;

                        if (anyCommand == null)
                            throw new InvalidOperationException("anyCommand have to be converted in SACommand, if the Async - Method is used");

                        try
                        {
                            var asyncResult =
                                anyCommand.BeginExecuteNonQuery();

                            while (!(asyncResult.IsCompleted))
                                Thread.Sleep(new TimeSpan(5)); //five nanoseconds

                            anyCommand.EndExecuteNonQuery(asyncResult);
                        }
                        catch (SAException ex) //BeginExecuteQuery
                        {
                            throw ex;
                        }
                        catch (ArgumentException ex) //EndExecuteNonQuery
                        {
                            throw ex;
                        }
                        catch (InvalidOperationException ex) //EndExecuteNonQuery
                        {
                            throw ex;
                        }
                    }
                    break;
                default:
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SAException ex)
                    {
                        throw ex;
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    break;
            }

            return QueryState.None;
        }

        public static void Drop()
        {

        }
    }

}
