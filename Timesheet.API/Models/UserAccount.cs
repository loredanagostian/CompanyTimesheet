namespace Timesheet.API.Models
{
    public class UserAccount
    {
        public int UserAccountId { get; set; }
        public int EmployeeId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
