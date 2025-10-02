using System.Text.RegularExpressions;
using Timesheet.API.Extensions;
using Timesheet.API.Models.DTOs;

namespace Timesheet.API.Validations
{
    public class UserAccountValidation
    {
        private static readonly Regex EmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static Dictionary<string, string[]> Validate(CreateUserAccountDto dto)
        {
            var errors = new Dictionary<string, string[]>();

            var email = dto.Email.TrimToNull();
            var password = dto.Password?.Trim(); // allow spaces inside passwords but trim ends

            // Email: optional, but if present must match regex
            if (email is not null && !EmailRegex.IsMatch(email))
            {
                errors["email"] = ["Email is not a valid email address."];
            }

            // Password: optional, but if present must be >= 8 chars
            if (password is not null && password.Length < 8)
            {
                errors["password"] = ["Password must be at least 8 characters long."];
            }

            return errors;
        }
    }
}
