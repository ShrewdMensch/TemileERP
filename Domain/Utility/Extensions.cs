
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Utility
{
    public static class Extensions
    {
        public static bool IsMorethanAnHourAgo(this DateTime theDateTime)
        {
            DateTime now = DateTime.Now;
            TimeSpan diff = now - theDateTime;

            return diff.TotalHours > 1.0;
        }
        public static bool IsAway(this DateTime theDateTime)
        {
            DateTime now = DateTime.Now;
            TimeSpan diff = now - theDateTime;

            return diff.TotalSeconds > 1.0;
        }

        public static string GetInitial(this string theString)
        {
            return (String.IsNullOrEmpty(theString)) ? "" : (theString.Substring(0, 1) + ".");

        }
        public static string Combine(this string theString, string firstString, string secondString)
        {
            return theString + " " + firstString + " " + secondString;

        }

        public static int AllDaysUntil(this DateTime firstDay, DateTime lastDay)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;

            if (firstDay > lastDay)
                return 0;

            TimeSpan span = lastDay - firstDay;

            return span.Days + 1;

        }

        public static int BusinessDaysUntil(this DateTime startDate, DateTime endDate)
        {
            Func<DateTime, bool> workDay = currentDate =>
                    (
                        currentDate.Date.DayOfWeek != DayOfWeek.Saturday &&
                        currentDate.Date.DayOfWeek != DayOfWeek.Sunday
                    );

            return Enumerable.Range(0, 1 + (endDate.Date - startDate.Date).Days).Count(
                intDay => workDay(startDate.AddDays(intDay)));
        }

        public static int BusinessDaysUntil(this DateTime startDate, DateTime endDate, IEnumerable<DateTime> lstExcludedDates)
        {
            Func<DateTime, bool> workDay = currentDate =>
                    (
                        currentDate.Date.DayOfWeek != DayOfWeek.Saturday &&
                        currentDate.Date.DayOfWeek != DayOfWeek.Sunday &&
                        !lstExcludedDates.Contains(currentDate.Date)
                    );

            return Enumerable.Range(0, 1 + (endDate.Date - startDate.Date).Days).Count(
                intDay => workDay(startDate.AddDays(intDay)));
        }
    }
}