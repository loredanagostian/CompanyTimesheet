using System.ComponentModel.DataAnnotations;

namespace Timesheet.API.Models.DTOs
{
    public class CreateTimeEntryDto
    {
        [Required]
        public required int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, 24)]
        public int HoursWorked { get; set; }
    }
}
