using System.ComponentModel.DataAnnotations;

namespace Timesheet.API.Models.DTOs
{
    public class CreateEmployeeDto
    {
        [Required]
        public required string CNP { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; }

        [Required]
        public required ContractType ContractType { get; set; }
    }
}
