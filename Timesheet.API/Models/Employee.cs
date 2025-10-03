namespace Timesheet.API.Models
{
    public enum ContractType { FullTime, PartTime, Contractor }

    public class Employee
    {
        public int EmployeeId { get; set; }
        public string CNP { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName  { get; set; } = string.Empty;
        public ContractType ContractType { get; set; }
        public List<UserAccount>? UserAccounts { get; set; }
        public List<TimeEntry>? TimeEntries { get; set; }
    }
}
