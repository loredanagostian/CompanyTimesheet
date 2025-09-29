using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timesheet.API.Entities
{
    public class TimeEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required int TimeEntryId { get; set; } // PK

        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; init; } // FK

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, 24)]
        public int HoursWorked { get; set; }
    }
}
