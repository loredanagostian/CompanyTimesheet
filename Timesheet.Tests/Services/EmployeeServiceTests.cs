using FluentAssertions;
using Moq;
using Timesheet.API.Models;
using Timesheet.API.Repositories.IRepositories;
using Timesheet.API.Services;
using Xunit;

namespace Timesheet.Tests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepoMock;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _employeeRepoMock = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_employeeRepoMock.Object);
        }

        [Fact]
        public async Task GetEmployees_ShouldReturnAll_WhenRepositoryReturnsData()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new() { EmployeeId = 1, FirstName = "Ana", LastName = "Ionescu", CNP = "123", ContractType = ContractType.FullTime },
                new() { EmployeeId = 2, FirstName = "Mihai", LastName = "Pop", CNP = "456", ContractType = ContractType.PartTime }
            };

            _employeeRepoMock
                .Setup(r => r.GetEmployees())
                .ReturnsAsync(employees);

            // Act
            var result = await _employeeService.GetEmployees();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().HaveCount(2);
            _employeeRepoMock.Verify(r => r.GetEmployees(), Times.Once);
        }
    }
}
