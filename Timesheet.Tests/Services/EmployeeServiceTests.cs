using FluentAssertions;
using Moq;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
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

        // GET Employees
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

        [Fact]
        public async Task GetEmployees_ShouldReturnEmpty_WhenRepositoryIsEmpty()
        {
            // Arrange
            _employeeRepoMock
                .Setup(repo => repo.GetEmployees())
                .ReturnsAsync(Enumerable.Empty<Employee>());

            // Act
            var result = await _employeeService.GetEmployees();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEmpty();

            _employeeRepoMock.Verify(repo => repo.GetEmployees(), Times.Once);
        }

        // CREATE Employee
        [Fact]
        public async Task CreateEmployee_ShouldReturnValidationError_WhenValidationFails()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                FirstName = "Ana123", // invalid because of numbers
                LastName = "",
                CNP = "abc", // invalid
                ContractType = "SomethingInvalid"
            };

            // Act
            var result = await _employeeService.CreateEmployee(dto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.HasValidationErrors.Should().BeTrue();
            result.ValidationErrors.Should().ContainKey("firstName");
            result.ValidationErrors.Should().ContainKey("cnp");
            result.ValidationErrors.Should().ContainKey("contractType");

            // Repo should NOT be called
            _employeeRepoMock.Verify(r => r.CreateEmployee(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task CreateEmployee_ShouldReturnFailure_WhenCnpAlreadyExists()
        {
            // Arrange
            var existingCnp = "1234567890123";

            var dto = new CreateEmployeeDto
            {
                FirstName = "Ana",
                LastName = "Ionescu",
                CNP = existingCnp,
                ContractType = "FullTime"
            };

            _employeeRepoMock
                .Setup(r => r.GetEmployeeByCNP(existingCnp))
                .ReturnsAsync(new Employee { CNP = existingCnp });

            // Act
            var result = await _employeeService.CreateEmployee(dto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.ErrorMessage.Should().Contain($"An employee with CNP {existingCnp} already exists.");

            _employeeRepoMock.Verify(r => r.CreateEmployee(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task CreateEmployee_ShouldCreateEmployee_WhenValidInput()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                FirstName = "Ana",
                LastName = "Ionescu",
                CNP = "1234567890123",
                ContractType = "FullTime"
            };

            _employeeRepoMock
                .Setup(r => r.GetEmployeeByCNP(dto.CNP))
                .ReturnsAsync((Employee?)null);

            _employeeRepoMock
                .Setup(r => r.CreateEmployee(It.IsAny<Employee>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _employeeService.CreateEmployee(dto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.CNP.Should().Be(dto.CNP);

            _employeeRepoMock.Verify(r => r.CreateEmployee(It.IsAny<Employee>()), Times.Once);
        }

        // GET Employee by ID
        [Fact]
        public async Task GetEmployeeById_ShouldReturnFailure_WhenIdNotFound()
        {
            // Arrange
            int employeeId = 1;
            _employeeRepoMock
                .Setup(r => r.GetEmployeeById(employeeId))
                .ReturnsAsync((Employee?)null);

            // Act
            var result = await _employeeService.GetEmployeeById(employeeId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.ErrorMessage.Should().Contain($"Employee with ID {employeeId} not found.");

            _employeeRepoMock.Verify(r => r.GetEmployeeById(employeeId), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeById_ShouldReturnEmployee_WhenIdFound()
        {
            // Arrange
            int employeeId = 1;
            var employee = new Employee
            {
                EmployeeId = employeeId,
                FirstName = "Ana",
                LastName = "Ionescu",
                CNP = "1234567890123",
                ContractType = ContractType.FullTime
            };

            _employeeRepoMock
                .Setup(r => r.GetEmployeeById(employeeId))
                .ReturnsAsync(employee);

            // Act
            var result = await _employeeService.GetEmployeeById(employeeId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.EmployeeId.Should().Be(employeeId);

            _employeeRepoMock.Verify(r => r.GetEmployeeById(employeeId), Times.Once);
        }

        // DELETE Employee
        [Fact]
        public async Task DeleteEmployee_ShouldReturnFailure_WhenIdNotFound()
        {
            // Arrange
            int employeeId = 1;
            _employeeRepoMock
                .Setup(r => r.GetEmployeeById(employeeId))
                .ReturnsAsync((Employee?)null);

            // Act
            var result = await _employeeService.DeleteEmployee(employeeId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("not found");

            _employeeRepoMock.Verify(r => r.GetEmployeeById(employeeId), Times.Once);
            _employeeRepoMock.Verify(r => r.DeleteEmployee(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldDeleteEmployee_WhenIdFound()
        {
            // Arrange
            int employeeId = 1;
            var employee = new Employee
            {
                EmployeeId = employeeId,
                FirstName = "Ana",
                LastName = "Ionescu",
                CNP = "1234567890123",
                ContractType = ContractType.FullTime
            };

            _employeeRepoMock
                .Setup(r => r.GetEmployeeById(employeeId))
                .ReturnsAsync(employee);

            _employeeRepoMock
                .Setup(r => r.DeleteEmployee(employee))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _employeeService.DeleteEmployee(employeeId);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _employeeRepoMock.Verify(r => r.GetEmployeeById(employeeId), Times.Once);
            _employeeRepoMock.Verify(r => r.DeleteEmployee(employee), Times.Once);
        }

        // GET Employee by CNP
        [Fact]
        public async Task GetEmployeeByCNP_ShouldReturnNull_WhenCnpNotFound()
        {
            // Arrange
            string cnp = "1234567890123";
            _employeeRepoMock
                .Setup(r => r.GetEmployeeByCNP(cnp))
                .ReturnsAsync((Employee?)null);

            // Act
            var result = await _employeeService.GetEmployeeByCNP(cnp);

            // Assert
            result.Should().BeNull();

            _employeeRepoMock.Verify(r => r.GetEmployeeByCNP(cnp), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeByCNP_ShouldReturnEmployee_WhenCnpExists()
        {
            // Arrange
            string cnp = "1234567890123";
            var employee = new Employee
            {
                EmployeeId = 1,
                FirstName = "Ana",
                LastName = "Ionescu",
                CNP = cnp,
                ContractType = ContractType.FullTime
            };
            _employeeRepoMock
                .Setup(r => r.GetEmployeeByCNP(cnp))
                .ReturnsAsync(employee);

            // Act
            var result = await _employeeService.GetEmployeeByCNP(cnp);

            // Assert
            result.Should().NotBeNull();
            result!.CNP.Should().Be(cnp);

            _employeeRepoMock.Verify(r => r.GetEmployeeByCNP(cnp), Times.Once);
        }
    }
}
