using System.Collections.Generic;
using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Domain;

namespace Utility
{
    public static class Extensions
    {
        /***********************************************************************************************************
        ******* DateTime Extension Methods*************************************************************************
        ************************************************************************************************************/
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

        public static string ToISO8601(this DateTime theDateTime)
        {
            return theDateTime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", DateTimeFormatInfo.InvariantInfo);
        }

        public static string ToFormalTime(this DateTime theDateTime)
        {
            return theDateTime.ToString("hh:mm tt");
        }

        public static string ToFormalDate(this DateTime theDateTime)
        {
            return theDateTime.ToString("dddd, MMMM, dd, yyyy");
        }

        public static string ToFormalShortDate(this DateTime theDateTime)
        {
            return theDateTime.ToString("MMMM, dd, yyyy");
        }
        public static string ToFormalShortDateWithTime(this DateTime theDateTime)
        {
            return theDateTime.ToString("MMMM, dd, yyyy @ hh:mm tt");
        }

        public static string ToFormalMonthAndYear(this DateTime theDateTime)
        {
            return theDateTime.ToString("MMMM, yyyy");
        }

        public static string ToFormalMonthAndDay(this DateTime theDateTime)
        {
            return theDateTime.ToString("MMMM, dd");
        }

        public static string ToFormalShortMonthAndDay(this DateTime theDateTime)
        {
            return theDateTime.ToString("MMM dd");
        }
        public static string ToFormalMonthAndOrdinalDay(this DateTime theDateTime)
        {
            if (theDateTime == null)
                return "";
            return String.Format("{0} {1}", theDateTime.ToFormalOrdinalDay(), theDateTime.ToString("MMM"));
        }

        public static string ToFormalMonth(this DateTime theDateTime)
        {
            return theDateTime.ToString("MMMM");
        }

        public static string ToFormalDay(this DateTime theDateTime)
        {
            return theDateTime.ToString("dddd");
        }

        public static string ToFormalOrdinalDay(this DateTime theDateTime)
        {
            return theDateTime.Day.Ordinal();
        }

        public static DateTime ToDateOnly(this DateTime theDateTime)
        {
            return new DateTime(theDateTime.Year, theDateTime.Month, theDateTime.Day);
        }
        public static DateTime GetFirstDayOfYear(this DateTime theDateTime)
        {
            return new DateTime(theDateTime.Year, 1, 1);
        }

        public static DateTime GetLastDayOfYear(this DateTime theDateTime)
        {
            var daysInMonth = DateTime.DaysInMonth(theDateTime.Year, theDateTime.Month);

            return new DateTime(theDateTime.Year, 12, 31);
        }

        public static DateTime GetFirstDayOfMonth(this DateTime theDateTime)
        {
            return new DateTime(theDateTime.Year, theDateTime.Month, 1);
        }

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
        public static bool HasSameMonthAndYearWith(this DateTime theDateTime, DateTime anotherDateTime)
        {
            return (theDateTime.Month == anotherDateTime.Month) && (theDateTime.Year == anotherDateTime.Year);
        }
        public static bool HasSameYearWith(this DateTime theDateTime, DateTime anotherDateTime)
        {
            return (theDateTime.Year == anotherDateTime.Year);
        }


        /***********************************************************************************************************
               ******* String Extension Methods*************************************************************************
               ************************************************************************************************************/
        public static string GetInitial(this string theString)
        {
            return (String.IsNullOrEmpty(theString)) ? "" : (theString.Substring(0, 1));

        }

        public static string ToTitleCase(this string theString)
        {
            var myTextInfo = new CultureInfo("en-US", false).TextInfo;

            return myTextInfo.ToTitleCase(theString);

        }

        public static HtmlString GetHtmlString(this string theString)
        {
            var htmlString = new HtmlString(theString);

            return htmlString;

        }

        /***********************************************************************************************************
               ******* int Extension Methods*************************************************************************
               ************************************************************************************************************/
        public static string Ordinal(this int number)
        {
            var numberString = number.ToString();
            if ((number % 100) == 11 || (number % 100) == 12 || (number % 100) == 13)
            {
                return numberString + "th";
            }
            switch (number % 10)
            {
                case 1: numberString += "st"; break;
                case 2: numberString += "nd"; break;
                case 3: numberString += "rd"; break;
                default: numberString += "th"; break;
            }
            return numberString;
        }

        /***********************************************************************************************************
               ******* double/currency Extension Methods*************************************************************************
               ************************************************************************************************************/
        public static string ToCurrency(this double number)
        {
            return number.ToString("C", new CultureInfo("en-NG", false));
        }

        public static string ToPercentageStr(this float number)
        {
            return String.Format("{0}%", number.ToString());
        }

        /***********************************************************************************************************
               ******* Collection Extension Methods*************************************************************************
               ************************************************************************************************************/
        public static Payroll GetCurrentPayroll(this IEnumerable<Payroll> payrolls)
        {
            var today = DateTime.Today.ToFormalMonthAndYear();

            return payrolls.FirstOrDefault(p => p.Date.ToFormalMonthAndYear() == today);
        }

    }
}