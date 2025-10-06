namespace Timesheet.API.Models
{
    public class TimeEntry
    {
        public int TimeEntryId { get; set; }
        public int EmployeeId { get; set; }
        public DateOnly Date { get; set; }
        public int HoursWorked { get; set; }
        public Employee? Employee { get; set; }
    }
}
