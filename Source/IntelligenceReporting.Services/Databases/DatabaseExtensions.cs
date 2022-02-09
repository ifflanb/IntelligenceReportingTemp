using System.Data.Common;

namespace IntelligenceReporting.Databases
{
    public static class DatabaseExtensions
    {
        public static readonly DateTime DateZero = new(1900, 01, 01);

        public static string FormatSql(this DateTime date) => date.TimeOfDay == TimeSpan.Zero ? $"'{date:yyyy-MM-dd}'" : $"'{date:yyyy-MM-dd HH:mm:ss.fff}'";
        public static string FormatSql(this DateTime? date) => !date.HasValue ? "null" : date.Value.FormatSql();
        public static string FormatSql(this string? text) => text == null ? "null" : $"'{text.Replace("'", "''")}'";

        public static string AppendSqlWhere(this string where, bool condition, string expression)
            => !condition ? where : $"\r\n    {(where == "" ? "WHERE" : "AND")} {expression}";

        public static decimal? GetNullableDecimal(this DbDataReader reader, int field) => reader.IsDBNull(field) ? null : reader.GetDecimal(field);
        public static DateTime? GetNullableDate(this DbDataReader reader, int field) => reader.IsDBNull(field) ? null : reader.GetDateTime(field);
        public static DateTimeOffset? GetNullableDateTime(this DbDataReader reader, int field)
        {
            var date = GetNullableDate(reader, field);
            if (date == null || date < DateZero) return null;
            return date;
        }

        public static int? GetNullableInt32(this DbDataReader reader, int field) => reader.IsDBNull(field) ? null : reader.GetInt32(field);
        public static string? GetNullableString(this DbDataReader reader, int field) => reader.IsDBNull(field) ? null : reader.GetString(field);
    }
}
