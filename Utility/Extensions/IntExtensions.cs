using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Extensions
{
    public static class IntExtensions
    {
        public static string Ordinal(this int number)
        {
            var numberString = number.ToString();

            if ((number % 100) == 11 || (number % 100) == 12 || (number % 100) == 13)
            {
                return $"{numberString}th";
            }

            numberString += (number % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th",
            };
            return numberString;
        }

        public static string ToDays(this int number) => string.Format("{0} {1}", number.ToString(), (number < 2) ? "day" : "days");

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
    }
}
