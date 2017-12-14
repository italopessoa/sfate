using System;
using System.Runtime.InteropServices;

namespace BAU.Api.Utils
{
    /// <summary>
    /// DateTime extension methods
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Weekend days
        /// </summary>
        private const DayOfWeek WEEKEND_DAYS_FLAGS = (DayOfWeek.Sunday | DayOfWeek.Saturday);
        private const int WEEK_TOTAL_DAYS = 7;

        /// <summary>
        /// Next business day of the week
        /// </summary>
        /// <param name="today">Actual date</param>
        /// <returns>Next business day date</returns>
        public static DateTime NextBusinessDay(this DateTime today)
        {
            if (today.AddDays(1).DayOfWeek.HasFlag(WEEKEND_DAYS_FLAGS))
            {
                return NextDayOfWeek(today, DayOfWeek.Monday, 1);
            }
            return today.AddDays(1);
        }

        /// <summary>
        /// Previous business day of the week
        /// </summary>
        /// <param name="today">Actual date</param>
        /// <returns>Previous business day date</returns>
        public static DateTime PreviousBusinessDay(this DateTime today)
        {
            if (today.AddDays(-1).DayOfWeek == DayOfWeek.Saturday || today.AddDays(-1).DayOfWeek == DayOfWeek.Sunday)
            {
                return PreviousDayOfWeek(today, DayOfWeek.Friday);
            }
            return today.AddDays(-1).Date;
        }

        /// <summary>
        /// Get a specific next day of a week
        /// </summary>
        /// <param name="today">Actua date</param>
        /// <param name="targetDayOfWeek">Target day of the week</param>
        /// <param name="weeksAhead">Number of weeks ahead to look up for the day</param>
        /// <returns>Next target day of the week</returns>
        public static DateTime NextDayOfWeek(this DateTime today, DayOfWeek targetDayOfWeek, [Optional] int weeksAhead)
        {
            int _weeksAhead = weeksAhead;
            if (today.DayOfWeek == targetDayOfWeek && _weeksAhead == 0)
            {
                _weeksAhead = 1;
            }
            else if (today.DayOfWeek > targetDayOfWeek)
            {
                if(_weeksAhead == 0) _weeksAhead = 1;
                return today.AddDays(_weeksAhead * WEEK_TOTAL_DAYS - (today.DayOfWeek - targetDayOfWeek)).Date;
            }
            return today.AddDays((targetDayOfWeek - today.DayOfWeek) + _weeksAhead * WEEK_TOTAL_DAYS).Date;
        }

        /// <summary>
        /// Get a specific previous day of a week
        /// </summary>
        /// <param name="today">Actua date</param>
        /// <param name="targetDayOfWeek">Target day of the week</param>
        /// <param name="weeksAgo">Number of weeks ago to look up for the day</param>
        /// <returns>Previous target day of the week</returns>
        public static DateTime PreviousDayOfWeek(this DateTime today, DayOfWeek targetDayOfWeek, [Optional] int weeksAgo)
        {
            if (today.DayOfWeek > targetDayOfWeek)
            {
                return today.AddDays(targetDayOfWeek - today.DayOfWeek - (weeksAgo * WEEK_TOTAL_DAYS)).Date;
            }
            if (today.DayOfWeek == targetDayOfWeek)
            {
                if (weeksAgo == 0)
                    return today.AddDays(-WEEK_TOTAL_DAYS);
                else
                    return today.AddDays(-(weeksAgo * WEEK_TOTAL_DAYS));
            }
            int _weeksAgo = weeksAgo == 0 ? 1 : weeksAgo;
            return today.AddDays(-((_weeksAgo * WEEK_TOTAL_DAYS) - (targetDayOfWeek - today.DayOfWeek))).Date;
        }
    }
}
