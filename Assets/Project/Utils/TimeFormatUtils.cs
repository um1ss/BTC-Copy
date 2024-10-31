using System;

namespace Project.Utils
{
    public static class TimeFormatUtils
    {
        public static string GetTimerTextShort(DateTimeOffset now, DateTimeOffset end, Func<string> textDataForPastTime) {
            return GetTimerTextShort(end - now, textDataForPastTime);
        }

        public static string GetTimerTextShort(TimeSpan timeLeft, Func<string> textForPastTime) {
            if (timeLeft <= TimeSpan.Zero) {
                return textForPastTime();
            }

            return GetTimerDaysOrHourMinSec(timeLeft);
        }

        public static string GetTimerDaysOrHourMinSec(TimeSpan timeLeft) {
            return timeLeft.TotalDays >= 1
                ? $"{(int) Math.Ceiling(timeLeft.TotalDays)} d"
                : FormatMaybeHourMinSec(timeLeft);
        }

        public static string FormatMaybeHourMinSec(this TimeSpan span) {
            return span.TotalHours >= 1.0
                ? FormatTotalHourMinSec(span)
                : FormatMinSec(span);
        }

        public static string FormatTotalHourMinSec(this TimeSpan span) {
            return $"{(int) span.TotalHours:00}:{span.Minutes:00}:{span.Seconds:00}";
        }

        public static string FormatMinSec(this TimeSpan span) {
            return $"{span.Minutes:00}:{span.Seconds:00}";
        }
    }
}