namespace Timesheet.API.Models
{
    public class AuthUser
    {
        public string Email { get; init; } = default!;
        public int EmployeeId { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }

        // Optional convenience
        public string FullName =>
            string.Join(" ", new[] { FirstName, LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));

        public AuthUser() { } // for serializers

        public AuthUser(string email, int employeeId, string? firstName = null, string? lastName = null)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
