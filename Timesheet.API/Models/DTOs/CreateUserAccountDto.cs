namespace Timesheet.API.Models.DTOs
{
    public class CreateUserAccountDto
    {
        public int EmployeeId { get; set; }

        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
