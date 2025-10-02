namespace Timesheet.API.Extensions
{
    public static class StringExtensions
    {
        public static string CapitalizeFirstLetter(this string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.Trim();

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        public static string? TrimToNull(this string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return s.Trim();
        }

        public static string? ToLowerInvariantSafe(this string? s)
        {
            return s is null ? null : s.ToLowerInvariant();
        }
    }
}
