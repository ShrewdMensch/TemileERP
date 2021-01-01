using System.Globalization;

namespace Utility.Extensions
{
    public static class DoubleExtensions
    {
        public static string ToCurrency(this double number) => number.ToString("C", new CultureInfo("en-NG", false));
    }
}
