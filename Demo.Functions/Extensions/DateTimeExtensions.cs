using System;

namespace Demo.Functions.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToAEST(this DateTime utcDateTime)
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            var localNow = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tz);
            return localNow;
        }
    }
}
