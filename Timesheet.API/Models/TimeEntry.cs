namespace Timesheet.API.Models
{
    public class TimeEntry
    {
        public Guid TimeEntryId { get; init; }
        public required UserAccount UserAccount { get; set; }
        public DateTime Date { get; set; }
        public int HoursWorked { get; set; }
    }
}
