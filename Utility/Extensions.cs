using System.Collections.Generic;
using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Domain;
using Utility.DTOs;
using Utility.Comparer;
using Domain.Utility;

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
            if (string.IsNullOrWhiteSpace(theString)) return null;

            var myTextInfo = new CultureInfo("en-US", false).TextInfo;

            return myTextInfo.ToTitleCase(theString);

        }

        public static HtmlString GetHtmlString(this string theString)
        {
            var htmlString = new HtmlString(theString);

            return htmlString;

        }

        public static string CombineAsDateRange(this string firstDateStr, string secondDateStr)
        {
            if (DateTime.Parse(firstDateStr).Date == DateTime.Parse(secondDateStr).Date) return firstDateStr;

            return firstDateStr + " to " + secondDateStr;

        }

        public static string[] SplitBySpace(this string theString)
        {
            return theString.Split(' ', 2);

        }

        public static DateRange ToDateRange(this string dateRangeStr)
        {
            if (dateRangeStr == null) return null;

            DateTime startDate, endDate;

            if (dateRangeStr.IndexOf("to") >= 0)
            {
                var dateRangeArray = dateRangeStr.Split("to");
                startDate = DateTime.Parse(dateRangeArray[0].Trim());
                endDate = DateTime.Parse(dateRangeArray[1].Trim());
            }

            else
            {
                startDate = DateTime.Parse(dateRangeStr.Trim());
                endDate = DateTime.Parse(dateRangeStr.Trim());
            }

            var dateRange = new DateRange
            {
                StartDate = startDate,
                EndDate = endDate
            };

            return dateRange;

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

        public static string ToDays(this int number)
        {
            return string.Format("{0} {1}", number.ToString(), (number < 2) ? "day" : "days");
        }
        public static string ToSentFrequency(this int number)
        {
            if (number < 1)
            {
                return "Never Sent";
            }

            else if (number == 1)
            {
                return "Sent Once";
            }
            else
            {
                return string.Format("Sent {0} times", number.ToString());
            }
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

        public static IEnumerable<InstructionToBankListDto> DistinctByVessel(this IEnumerable<InstructionToBankListDto> instructionToBankDtos)
        {
            return instructionToBankDtos.Distinct(new InstructionToBankListDtoComparer());
        }
    }
}