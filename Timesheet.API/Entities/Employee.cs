using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Timesheet.API.Models;

namespace Timesheet.API.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required int EmployeeId { get; init; }

        [Required]
        public required string CNP { get; init; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public ContractType ContractType { get; set; }

        public List<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();

        public List<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
    }
}
