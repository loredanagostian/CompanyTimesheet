namespace Timesheet.API.Models
{
    public class UserAccount
    {
        public int EmployeeId { get; init; } // FK
        public required string Email { get; set; } // PK
        public required string Password { get; set; }
    }
}
