namespace Timesheet.API.Models
{
    public enum ContractType { FullTime, PartTime, Contractor }

    public class Employee
    {
        public Guid EmployeeId { get; init; }
        public int EmployeeIdNumber { get; init; }
        public required string FirstName { get; set; }
        public required string LastName  { get; set; }
        public required ContractType ContractType { get; set; }
        public List<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();
    }
}
