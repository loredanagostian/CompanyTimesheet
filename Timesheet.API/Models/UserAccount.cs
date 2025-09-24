namespace Timesheet.API.Models
{
    public class UserAccount
    {
        public Guid UserId { get; init; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required Employee Employee { get; set; }
    }
}
