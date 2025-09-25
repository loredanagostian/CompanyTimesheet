namespace Timesheet.API.Models
{
    public enum ContractType { FullTime, PartTime, Contractor }

    public class Employee
    {
        public required int EmployeeId { get; init; } // PK
        public required string CNP { get; init; } // FK
        public string FirstName { get; set; } = string.Empty;
        public string LastName  { get; set; } = string.Empty;
        public ContractType ContractType { get; set; }
        public List<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();
        public List<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();

        // TODO
        public int TotalHoursWorkedPerWeek => 0;
        public int TotalDaysWorkedPerMonth => 0;
    }
}
