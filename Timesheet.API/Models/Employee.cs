using static Timesheet.API.Constants.Enums;

namespace Timesheet.API.Models
{
    public class Employee
    {
        public Guid EmployeeId { get; init; }
        public int EmployeeIdNumber { get; init; }
        public required string FirstName { get; set; }
        public required string LastName  { get; set; }
        public required ContractType ContractType { get; set; }
    }
}
