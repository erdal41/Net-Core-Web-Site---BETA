using System;

namespace MySite.Shared.Utilities.Extensions
{
    public static class DateTimeExtensions
    {
        public static string FullDateAndTimeStringWithUnderscore(this DateTime dateTime)
        {
            return
                $"{dateTime.Millisecond}{dateTime.Second}{dateTime.Minute}{dateTime.Hour}{dateTime.Day}{dateTime.Month}{dateTime.Year}";
        }
    }
}