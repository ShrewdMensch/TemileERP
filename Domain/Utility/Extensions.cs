using System;

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
    }
}