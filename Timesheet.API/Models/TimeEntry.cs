namespace Timesheet.API.Models
{
    public class TimeEntry
    {
        public Guid TimeEntryId { get; init; } // PK
        public int EmployeeId { get; init; } // FK
        public DateTime Date { get; set; }
        public int HoursWorked { get; set; }
    }
}
