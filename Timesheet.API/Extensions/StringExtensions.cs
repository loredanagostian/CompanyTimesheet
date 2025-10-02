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
    }
}
