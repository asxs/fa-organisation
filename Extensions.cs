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

using System.Windows.Forms;

namespace IxSApp
{
    using LvUnits;

    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Compares the with case.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareValue">The compare value.</param>
        /// <returns></returns>
        public static bool CompareWithCase(this string value, string compareValue)
        {
            var result =
                false;

            for (var i = 0; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]))
                    if (i < compareValue.Length && char.IsUpper(compareValue[i]))
                    {
                        if (value[i] == compareValue[i])
                            result = true;
                    }

                if (result)
                    break;

                if (!(result))
                    if (!char.IsLower(value[i]))
                        if (i < compareValue.Length && char.IsLower(compareValue[i]))
                        {
                            if (value[i] == compareValue[i])
                                result = true;
                        }

                if (result)
                    break;
            }

            return result;
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="unit">The unit.</param>
        /// <returns></returns>
        public static ListViewItem Add(this ListView item, ListViewItemUnit unit)
        {
            return item.Items.Add(unit as ListViewItem);
        }

        /// <summary>
        /// Subtracts the time span without weekends.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareTime">The compare time.</param>
        /// <returns></returns>
        public static int SubtractTimeSpanWithoutWeekends(this DateTime value, DateTime compareTime)
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


        /// <summary>
        /// Subtracts the time span without weekends.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareTime">The compare time.</param>
        /// <returns></returns>
        public static int SubtractTimeSpanWithoutWeekends(this SqlDateTime value, SqlDateTime compareTime)
        {
            return value.Value.SubtractTimeSpanWithoutWeekends(compareTime.Value);
        }

        /// <summary>
        /// To the time stamp.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToTimeStamp(this SqlDateTime value)
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
