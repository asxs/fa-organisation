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
    using Data;

    public sealed class FirmDataUnit : IWorkDataUnit
    {
        public FirmDataUnit()
        {

        }

        #region FirmDataUnit (IDataUnit)

        public UnitPriorityType Priority { get; set; }

        public IDbCommand Command { get; set; }

        public string Table
        {
            get
            {
                return "ASXS_FIRM";
            }
        }

        public string Database { get; set; }

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

        public QueryStateType Remove(Units package, long id = 0)
        {
            return QueryStateType.None;
        }

        #endregion
    }
}
