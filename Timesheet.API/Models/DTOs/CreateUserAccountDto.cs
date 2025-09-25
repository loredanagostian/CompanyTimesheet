using System.ComponentModel.DataAnnotations;

namespace Timesheet.API.Models.DTOs
{
    public class CreateUserAccountDto
    {
        [Required]
        public int EmployeeId { get; set; }
    }
}
