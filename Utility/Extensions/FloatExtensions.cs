using System;

namespace Utility.Extensions
{
    public static class FloatExtensions
    {
        public static string ToPercentageStr(this float number) => String.Format("{0}%", number.ToString());
    }
}
