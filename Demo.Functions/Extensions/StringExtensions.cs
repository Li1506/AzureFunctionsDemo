using System.Globalization;

namespace Demo.Functions.Extensions
{
    public static class StringExtensions
    {
        public static decimal ToDecimal(this string currency)
        {
            if (string.IsNullOrEmpty(currency)) return 0m;

            return decimal.Parse(currency, NumberStyles.Currency);
        }
    }
}
