namespace IntelligenceReporting.Extensions
{
    /// <summary>Extensions for DateTime</summary>
    public static class DateTimeExtensions
    {
        /// <summary>Get the start of the month from a date</summary>
        public static DateTime StartOfMonth(this DateTime date) => new(date.Year, date.Month, 1);

        /// <summary>Get the end of the month from a date</summary>
        public static DateTime EndOfMonth(this DateTime date) => date.StartOfMonth().AddMonths(1).AddDays(-1);
    }
}
