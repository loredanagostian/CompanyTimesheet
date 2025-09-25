using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timesheet.API.Entities
{
    public class UserAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required string Email { get; set; }

        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; init; }

        [Required]
        public required string Password { get; set; }
    }
}
