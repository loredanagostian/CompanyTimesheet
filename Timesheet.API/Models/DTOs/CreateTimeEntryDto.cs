namespace Timesheet.API.Models.DTOs
{
    public class CreateTimeEntryDto
    {
        public int EmployeeId { get; set; }

        public DateTime Date { get; set; } // from calendar picker

        public int HoursWorked { get; set; } // 0..8
    }
}
