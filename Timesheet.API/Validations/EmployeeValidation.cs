using System.Text.RegularExpressions;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Validations
{
    public static class EmployeeValidation
    {
        // Letters (Unicode) + diacritics + spaces/apostrophes/hyphens
        private static readonly Regex NameRegex = new(@"^[\p{L}\p{M}'\- ]+$", RegexOptions.Compiled);
        private static readonly Regex CnpRegex = new(@"^\d{13}$", RegexOptions.Compiled);

        private static readonly HashSet<string> AllowedContractTypes =
            new(StringComparer.OrdinalIgnoreCase) { "FullTime", "PartTime", "Contractor" };

        public static Dictionary<string, string[]> Validate(CreateEmployeeDto dto)
        {
            var errors = new Dictionary<string, string[]>();

            string? first = dto.FirstName.Trim();
            string? last = dto.LastName.Trim();
            string? cnp = dto.CNP.Trim();
            string? type = dto.ContractType.Trim();

            if (string.IsNullOrWhiteSpace(first) || !NameRegex.IsMatch(first))
                errors["firstName"] = ["First name may contain letters, spaces, apostrophes, and hyphens only."];

            if (string.IsNullOrWhiteSpace(last) || !NameRegex.IsMatch(last))
                errors["lastName"] = ["Last name may contain letters, spaces, apostrophes, and hyphens only."];

            if (string.IsNullOrWhiteSpace(cnp) || !CnpRegex.IsMatch(cnp))
                errors["cnp"] = ["CNP must be exactly 13 digits."];

            if (string.IsNullOrWhiteSpace(type) || !AllowedContractTypes.Contains(type))
                errors["contractType"] = ["ContractType must be one of: FullTime, PartTime, Contractor."];

            return errors;
        }
    }

}
