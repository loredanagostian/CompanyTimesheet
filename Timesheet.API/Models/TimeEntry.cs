namespace Timesheet.API.Models
{
    public class TimeEntry
    {
        public int TimeEntryId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public int HoursWorked { get; set; }
    }
}
