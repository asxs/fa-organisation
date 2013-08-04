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
    public sealed class FirmDataUnit : IDataUnit
    {
        public FirmDataUnit()
        {

        }

        #region FirmDataUnit (IDataUnit)

        public DataUnitPriority Priority { get; set; }

        public IDbCommand Command { get; set; }

        public string Table
        {
            get
            {
                return "ASXS_FIRM";
            }
        }

        public string Database { get; set; }

        public QueryState Insert(DataUnitPackage units, long id = 0)
        {
            var state = QueryState.None;
            try
            {
                state = SqlQuery.Insert
                (
                    new QueryParameter()
                    {
                        Command = Command,
                        CommandText = units.Firm.ToSqlString(StatementType.Insert, units, id),
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

        public QueryState Update(DataUnitPackage units, long id = 0)
        {
            var state = QueryState.None;
            try
            {
                state = SqlQuery.Update
                (
                    new QueryParameter()
                    {
                        Command = Command,
                        CommandText = units.Firm.ToSqlString(StatementType.Update, units, id),
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

        public QueryState Remove(DataUnitPackage package, long id = 0)
        {
            return QueryState.None;
        }

        #endregion
    }
}
