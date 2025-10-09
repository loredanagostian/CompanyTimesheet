using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Timesheet.API.Models;
using Xunit;

namespace Timesheet.Tests.Controllers
{
    public class EmployeesControllerTests : IClassFixture<TestingWebAppFactory>
    {
        private readonly HttpClient _client;
        private readonly TestingWebAppFactory _factory;

        public EmployeesControllerTests(TestingWebAppFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetEmployees_ReturnsOkWithList()
        {
            // Act
            var resp = await _client.GetAsync("/api/timesheet/Employees");
            resp.EnsureSuccessStatusCode();

            // Use the same JSON options as the API (string enums)
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };

            // Assert API response first (optional)
            var data = await resp.Content.ReadFromJsonAsync<List<Employee>>(jsonOptions);
            data.Should().NotBeNull();
            data.Should().HaveCountGreaterThan(0);
            data.Any(e => e.FirstName == "Ana").Should().BeTrue();

            var all = _factory.UsingDb(db => db.Employees.ToList());
            all.Should().HaveCountGreaterThan(0);
        }
    }
}
