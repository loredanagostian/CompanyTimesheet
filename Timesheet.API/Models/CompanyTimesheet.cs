using static Timesheet.API.Constants.Enums;

namespace Timesheet.API.Models
{
    public class CompanyTimesheet
    {
        // Employee Info
        public int EmployeeIdNumber { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public ContractType ContractType { get; set; }

        // Time Entries
        public required List<TimeEntry> TimeEntries { get; set; }

        // Summary
        public int TotalHoursWorked => TimeEntries.Sum(te => te.HoursWorked);
        public int TotalDaysWorked => TimeEntries.Select(te => te.Date.Date).Distinct().Count();
    }
}
