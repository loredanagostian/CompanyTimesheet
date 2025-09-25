namespace Timesheet.API.Models
{
    public enum ContractType { FullTime, PartTime, Contractor }

    public class EmployeeModel
    {
        public required int EmployeeId { get; init; } // PK
        public required string CNP { get; init; } // FK
        public string FirstName { get; set; } = string.Empty;
        public string LastName  { get; set; } = string.Empty;
        public ContractType ContractType { get; set; }
        public List<UserAccountModel> UserAccounts { get; set; } = new List<UserAccountModel>();
        public List<TimeEntryModel> TimeEntries { get; set; } = new List<TimeEntryModel>();

        public int TotalHoursWorkedPerWeek
        {
            get
            {
                // Group time entries by week (ISO 8601: Monday as first day)
                return TimeEntries
                    .Where(te => te.Date.Year == DateTime.Now.Year)
                    .GroupBy(te => System.Globalization.ISOWeek.GetWeekOfYear(te.Date))
                    .OrderByDescending(g => g.Key)
                    .FirstOrDefault()?.Sum(te => te.HoursWorked) ?? 0;
            }
        }

        public int TotalDaysWorkedPerMonth
        {
            get
            {
                // Count distinct days worked in the current month
                var now = DateTime.Now;
                return TimeEntries
                    .Where(te => te.Date.Year == now.Year && te.Date.Month == now.Month)
                    .Select(te => te.Date.Date)
                    .Distinct()
                    .Count();
            }
        }
    }
}
