using Domain.Utility;
using Microsoft.AspNetCore.Html;
using System;
using System.Globalization;

namespace Utility.Extensions
{
    public static class StringExtensions
    {
        public static string GetInitial(this string theString) => String.IsNullOrEmpty(theString) ? "" : (theString.Substring(0, 1));

        public static string ToTitleCase(this string theString)
        {
            if (string.IsNullOrWhiteSpace(theString))
            {
                return null;
            }

            var myTextInfo = new CultureInfo("en-US", false).TextInfo;

            return myTextInfo.ToTitleCase(theString);

        }

        public static HtmlString GetHtmlString(this string theString) => new HtmlString(theString);

        public static string CombineAsDateRange(this string firstDateStr, string secondDateStr)
        {
            if (DateTime.Parse(firstDateStr).Date == DateTime.Parse(secondDateStr).Date) return firstDateStr;

            return firstDateStr + " to " + secondDateStr;

        }

        public static string[] SplitBySpace(this string theString) => theString.Split(' ', 2);

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

            return new DateRange { StartDate = startDate, EndDate = endDate };
        }
    }
}
