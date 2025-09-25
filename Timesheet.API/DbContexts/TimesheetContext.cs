using Microsoft.EntityFrameworkCore;
using Timesheet.API.Entities;
using Timesheet.API.Models;

namespace Timesheet.API.DbContexts
{
    public class TimesheetContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        public TimesheetContext(DbContextOptions<TimesheetContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasData(
                    new EmployeeModel
                    {
                        EmployeeId = 1,
                        FirstName = "Ana",
                        LastName = "Blandiana",
                        ContractType = ContractType.FullTime,
                        CNP = "1234567890123"
                    },
                    new EmployeeModel
                    {
                        EmployeeId = 2,
                        FirstName = "Ion",
                        LastName = "Gladiatorul",
                        ContractType = ContractType.PartTime,
                        CNP = "9876543210987"
                    },
                    new EmployeeModel
                    {
                        EmployeeId = 3,
                        FirstName = "Maria",
                        LastName = "Ioana",
                        ContractType = ContractType.FullTime,
                        CNP = "4567891234567"
                    },
                    new EmployeeModel
                    {
                        EmployeeId = 4,
                        FirstName = "Catalin",
                        LastName = "Botezatul",
                        ContractType = ContractType.Contractor,
                        CNP = "7891234567891"
                    }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
