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
    public static class Extensions
    {
        public static int CalculateWaitTimeUntil(this DateTime value, DateTime compareTime)
        {
            var time = (value - compareTime);
            var waitDays
                = 0;

            if (time.Days > 0)
            {
                var i = 0;
                for (; i < time.Days; ++i)
                {
                    var day =
                        new DateTime(compareTime.Year, compareTime.Month, compareTime.Day);

                    day = day.AddDays(i);

                    if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                        waitDays++;
                }
            }

            return waitDays;
        }

        
        public static int CalculateWaitTimeUntil(this SqlDateTime value, SqlDateTime compareTime)
        {
            var time = (value.Value - compareTime.Value);
            var waitDays 
                = 0;

            if (time.Days > 0)
            {
                var i = 0;
                for (; i < time.Days; ++i)
                {
                    var day = 
                        new DateTime(compareTime.Value.Year, compareTime.Value.Month, compareTime.Value.Day + waitDays);

                    if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                        waitDays++;
                }
            }

            return waitDays;
        }

        public static string ToSaTimeStamp(this SqlDateTime value)
        {
            var dateTimeValue = value.ToSqlString();
            var dateTimeValues
                = dateTimeValue.Value.Split(new char[] { '.' });
            var year = string.Empty;

            if (dateTimeValues.Length > 0)
            {
                var dateTimeYear
                    = dateTimeValues[2].Split(new char[] { ' ' });

                if (dateTimeYear.Length > 0)
                    year = dateTimeYear[0];
            }

            return year + "/" + dateTimeValues[1] + "/" + dateTimeValues[0];
        }
    }
}
