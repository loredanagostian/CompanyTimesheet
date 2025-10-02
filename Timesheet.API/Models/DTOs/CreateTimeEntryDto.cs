using System.ComponentModel.DataAnnotations;

namespace Timesheet.API.Models.DTOs
{
    public class CreateTimeEntryDto
    {
        public int EmployeeId { get; set; }

        public DateTime Date { get; set; }

        public int HoursWorked { get; set; }
    }
}
