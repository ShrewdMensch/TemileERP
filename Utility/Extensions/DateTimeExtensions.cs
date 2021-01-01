using System;
using System.Globalization;

namespace Utility.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }

        public static bool IsChild(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return (age >= 0) && (age <= 5);
        }

        public static bool IsMiddleAge(this DateTime theDateTime)
        {
            var age = theDateTime.CalculateAge();

            return (age >= 6) && (age <= 64);
        }

        public static bool IsOldAge(this DateTime theDateTime)
        {
            var age = theDateTime.CalculateAge();

            return age > 65;
        }

        public static string ToShortDate(this DateTime theDateTime)
        {
            return theDateTime.ToString("yyyy-MM-dd");
        }

        public static string ToISO8601(this DateTime theDateTime) => theDateTime.ToUniversalTime()
            .ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", DateTimeFormatInfo.InvariantInfo);

        public static string ToFormalTime(this DateTime theDateTime) => theDateTime.ToString("hh:mm tt");

        public static string ToFormalDate(this DateTime theDateTime) => theDateTime.ToString("dddd, MMMM, dd, yyyy");

        public static string ToFormalShortDate(this DateTime theDateTime) => theDateTime.ToString("MMM. dd, yyyy");

        public static string ToFormalShortDateWithTime(this DateTime theDateTime) => theDateTime.ToString("MMMM dd, yyyy 'at' hh:mmtt");

        public static string ToFormalMonthAndYear(this DateTime theDateTime) => theDateTime.ToString("MMMM, yyyy");

        public static string ToFormalMonthAndDay(this DateTime theDateTime) => theDateTime.ToString("MMMM, dd");

        public static string ToFormalShortMonthAndDay(this DateTime theDateTime) => theDateTime.ToString("MMM dd");

        public static string ToFormalMonthAndOrdinalDay(this DateTime theDateTime)
        {
            if (theDateTime == null)
                return "";
            return String.Format("{0} {1}", theDateTime.ToFormalOrdinalDay(), theDateTime.ToString("MMM"));
        }

        public static string ToFormalMonth(this DateTime theDateTime) => theDateTime.ToString("MMMM");

        public static string ToFormalDay(this DateTime theDateTime) => theDateTime.ToString("dddd");

        public static string ToFormalOrdinalDay(this DateTime theDateTime) => theDateTime.Day.Ordinal();

        public static DateTime ToDateOnly(this DateTime theDateTime) => theDateTime.Date;

        public static DateTime GetFirstDayOfYear(this DateTime theDateTime)
        {
            return new DateTime(theDateTime.Year, 1, 1);
        }

        public static DateTime GetLastDayOfYear(this DateTime theDateTime) => new DateTime(theDateTime.Year, 12, 31);

        public static DateTime GetFirstDayOfMonth(this DateTime theDateTime) => new DateTime(theDateTime.Year, theDateTime.Month, 1);

        public static DateTime GetLastDayOfMonth(this DateTime theDateTime)
        {
            var daysInMonth = DateTime.DaysInMonth(theDateTime.Year, theDateTime.Month);

            return new DateTime(theDateTime.Year, theDateTime.Month, daysInMonth);
        }

        public static bool IsMorethan30DaysAgo(this DateTime theDateTime)
        {
            DateTime now = DateTime.Today;
            TimeSpan diff = now - theDateTime;

            return diff.TotalDays > 30.0;
        }

        public static bool IsMorethanAYearAgo(this DateTime theDateTime)
        {
            DateTime now = DateTime.Today;
            TimeSpan diff = now - theDateTime;

            return diff.TotalDays > 365.0;
        }

        public static bool IsMorethanAnHourAgo(this DateTime theDateTime)
        {
            DateTime now = DateTime.Now;
            TimeSpan diff = now - theDateTime;

            return diff.TotalHours > 1.0;
        }

        public static bool HasSameMonthAndYearWith(this DateTime theDateTime, DateTime anotherDateTime) => (
            theDateTime.Month == anotherDateTime.Month) && (theDateTime.Year == anotherDateTime.Year);

        public static bool HasSameYearWith(this DateTime theDateTime, DateTime anotherDateTime) => (
            theDateTime.Year == anotherDateTime.Year);
    }
}