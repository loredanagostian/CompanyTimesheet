using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Timesheet.API.DbContexts;
using Timesheet.API.Models;

namespace Timesheet.Tests
{
    public class TestingWebAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Remove existing DbContext registration (SQL Server)
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<TimesheetContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Replace it with an in-memory DB
                services.AddDbContext<TimesheetContext>(options =>
                    options.UseInMemoryDatabase("TimesheetTestDb"));

                // Build provider and seed data
                using var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TimesheetContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                if (!db.Employees.Any())
                {
                    db.Employees.AddRange(
                        new Employee { FirstName = "Ana", LastName = "Ionescu", CNP = "123", ContractType = ContractType.FullTime },
                        new Employee { FirstName = "Mihai", LastName = "Pop", CNP = "456", ContractType = ContractType.PartTime }
                    );
                    db.SaveChanges();
                }

                Console.WriteLine("=== Seeded Employees in TimesheetTestDb ===");
                foreach (var e in db.Employees.ToList())
                {
                    Console.WriteLine($"  ID={e.EmployeeId}, Name={e.FirstName} {e.LastName}, Contract={e.ContractType}");
                }
                Console.WriteLine("===========================================");
            });
        }

        // Optional helper to access the DB from tests
        public T UsingDb<T>(Func<TimesheetContext, T> func)
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TimesheetContext>();
            return func(db);
        }
    }
}