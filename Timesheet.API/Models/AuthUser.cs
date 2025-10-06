namespace Timesheet.API.Models
{
    public class AuthUser
    {
        public string Email { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // Optional convenience
        public string FullName =>
            string.Join(" ", new[] { FirstName, LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));

        public AuthUser() { } // for serializers

        public AuthUser(string email, int employeeId, string firstName = "", string lastName = "")
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
