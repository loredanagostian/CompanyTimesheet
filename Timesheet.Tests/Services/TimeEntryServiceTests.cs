using FluentAssertions;
using Moq;
using Timesheet.API.Models;
using Timesheet.API.Models.DTOs;
using Timesheet.API.Repositories.IRepositories;
using Timesheet.API.Services;
using Timesheet.API.Services.Interfaces;
using Xunit;

namespace Timesheet.Tests.Services
{
    public class TimeEntryServiceTests
    {
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<ITimeEntryRepository> _timeEntryRepoMock;
        private readonly TimeEntryService _timeEntryService;

        public TimeEntryServiceTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _timeEntryRepoMock = new Mock<ITimeEntryRepository>();
            _timeEntryService = new TimeEntryService(_employeeServiceMock.Object, _timeEntryRepoMock.Object);
        }

        // helpers
        private static Employee Emp(
            int id = 1, string first = "Ana", string last = "Ionescu",
            string cnp = "1234567890123", ContractType ct = ContractType.FullTime,
            IEnumerable<UserAccount>? accounts = null
        ) => new()
        {
            EmployeeId = id,
            FirstName = first,
            LastName = last,
            CNP = cnp,
            ContractType = ct,
            UserAccounts = accounts?.ToList()
        };

        // GET TimeEntries
        [Fact]
        public async Task GetTimeEntries_ShouldReturnAll_WhenRepositoryReturnsData()
        {
            // Arrange
            var timeEntries = new List<TimeEntry>
            {
                new()
                {
                    TimeEntryId = 1,
                    EmployeeId = 1,
                    Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
                    HoursWorked = 8
                },
                new()
                {
                    TimeEntryId = 2,
                    EmployeeId = 2,
                    Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)),
                    HoursWorked = 6
                }
            };

            _timeEntryRepoMock
                .Setup(r => r.GetTimeEntries())
                .ReturnsAsync(ServiceResult<IEnumerable<TimeEntry>>.Success(timeEntries));

            // Act
            var result = await _timeEntryService.GetTimeEntries();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().HaveCount(2);

            _timeEntryRepoMock.Verify(r => r.GetTimeEntries(), Times.Once);
        }

        [Fact]
        public async Task GetTimeEntries_ShouldReturnEmpty_WhenRepositoryIsEmpty()
        {
            // Arrange
            _timeEntryRepoMock
                .Setup(repo => repo.GetTimeEntries())
                .ReturnsAsync(ServiceResult<IEnumerable<TimeEntry>>.Success(Enumerable.Empty<TimeEntry>()));

            // Act
            var result = await _timeEntryService.GetTimeEntries();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEmpty();

            _timeEntryRepoMock.Verify(repo => repo.GetTimeEntries(), Times.Once);
        }

        // CREATE Time Entry
        [Fact]
        public async Task CreateTimeEntry_ShouldReturnFailure_WhenEmployeeNotFound()
        {
            // Arrange
            var empId = 42;

            _employeeServiceMock
                .Setup(service => service.GetEmployeeById(empId))
                .ReturnsAsync(ServiceResult<Employee>.Failure("not found"));

            var timeEntryDto = new CreateTimeEntryDto
            {
                EmployeeId = empId,
                Date = DateTime.Now,
                HoursWorked = 8
            };

            // Act
            var result = await _timeEntryService.CreateTimeEntry(timeEntryDto);

            //Assert
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.ErrorMessage.Should().Contain($"No Employee was found with ID {empId}");

            _timeEntryRepoMock.Verify(r => r.CreateTimeEntry(It.IsAny<TimeEntry>()), Times.Never);
        }

        [Fact]
        public async Task CreateTimeEntry_ShouldReturnValidationError_WhenValidationFails()
        {
            // Arrange
            var emp = Emp();

            _employeeServiceMock
                .Setup(service => service.GetEmployeeById(emp.EmployeeId))
                .ReturnsAsync(ServiceResult<Employee>.Success(emp));

            var timeEntryDto = new CreateTimeEntryDto
            {
                EmployeeId = emp.EmployeeId,
                Date = DateTime.Now.AddMonths(1), // date should be in current month
                HoursWorked = 9 // value should be 0..8
            };

            // Act
            var result = await _timeEntryService.CreateTimeEntry(timeEntryDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.HasValidationErrors.Should().BeTrue();
            result.ValidationErrors.Should().HaveCount(2);
            result.ValidationErrors.Should().ContainKey("hoursWorked");
            result.ValidationErrors.Should().ContainKey("date");

            _timeEntryRepoMock.Verify(r => r.CreateTimeEntry(It.IsAny<TimeEntry>()), Times.Never);
        }

        [Fact]
        public async Task CreateTimeEntry_ShouldReturnFailure_WhenDateAlreadyExists()
        {
            // Arrange
            int empId = 1;
            var existingDate = DateTime.Now;

            var employee = new Employee
            {
                EmployeeId = empId,
                FirstName = "Ana",
                LastName = "Ionescu",
                ContractType = ContractType.FullTime,
                TimeEntries = new List<TimeEntry>
                {
                    new() { Date = DateOnly.FromDateTime(existingDate) }
                }
            };

            _employeeServiceMock
                .Setup(s => s.GetEmployeeById(empId))
                .ReturnsAsync(ServiceResult<Employee>.Success(employee));

            var dto = new CreateTimeEntryDto
            {
                EmployeeId = empId,
                Date = existingDate // trying to create with same date
            };

            // Act
            var result = await _timeEntryService.CreateTimeEntry(dto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.ErrorMessage.Should().Contain(
                $"A Time Entry already exists for Employee with ID {empId} and for Date {DateOnly.FromDateTime(existingDate)}.");

            _timeEntryRepoMock.Verify(r => r.CreateTimeEntry(It.IsAny<TimeEntry>()), Times.Never);
        }

        [Fact]
        public async Task CreateTimeEntry_ShouldCreateTimeEntry_WhenValidInput()
        {
            // Arrange
            var employee = Emp();
            var empId = 1;

            _employeeServiceMock
               .Setup(s => s.GetEmployeeById(empId))
               .ReturnsAsync(ServiceResult<Employee>.Success(employee));

            var dto = new CreateTimeEntryDto
            {
                EmployeeId = empId,
                Date = DateTime.Now,
                HoursWorked = 8
            };

            // Act
            var result = await _timeEntryService.CreateTimeEntry(dto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.EmployeeId.Should().Be(empId);
            result.Data.Date.Should().Be(DateOnly.FromDateTime(DateTime.Now));
            result.Data.HoursWorked.Should().Be(8);

            _timeEntryRepoMock.Verify(r => r.CreateTimeEntry(It.IsAny<TimeEntry>()), Times.Once);
        }

        // GET Time Entry by ID
        [Fact]
        public async Task GetTimeEntryById_ShouldReturnFailure_WhenIdNotFound()
        {
            // Arrange
            var timeEntryId = 1;
            _timeEntryRepoMock
                .Setup(repo => repo.GetTimeEntryById(timeEntryId))
                .ReturnsAsync((TimeEntry?)null);

            // Act
            var result = await _timeEntryService.GetTimeEntryById(timeEntryId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.ErrorMessage.Should().Contain($"No Time Entry was found with ID {timeEntryId}.");

            _timeEntryRepoMock.Verify(r => r.GetTimeEntryById(timeEntryId), Times.Once);
        }

        [Fact]
        public async Task GetTimeEntryById_ShouldReturnTimeEntry_WhenIdFound()
        {
            // Arrange
            var timeEntryId = 1;
            var employee = Emp();

            var expectedTimeEntry = new TimeEntry
            {
                TimeEntryId = timeEntryId,
                EmployeeId = employee.EmployeeId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                HoursWorked = 8
            };

            _timeEntryRepoMock
                .Setup(repo => repo.GetTimeEntryById(timeEntryId))
                .ReturnsAsync(expectedTimeEntry);

            // Act
            var result = await _timeEntryService.GetTimeEntryById(timeEntryId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.TimeEntryId.Should().Be(timeEntryId);
            result.Data.EmployeeId.Should().Be(employee.EmployeeId);
            result.Data.Date.Should().Be(DateOnly.FromDateTime(DateTime.Now));
            result.Data.HoursWorked.Should().Be(8);
            
            _timeEntryRepoMock.Verify(r => r.GetTimeEntryById(timeEntryId), Times.Once);
        }

        // DELETE Time Entry
        [Fact]
        public async Task DeleteTimeEntry_ShouldReturnFailure_WhenIdNotFound()
        {
            // Arrange
            var timeEntryId = 1;
            _timeEntryRepoMock
                .Setup(repo => repo.GetTimeEntryById(timeEntryId))
                .ReturnsAsync((TimeEntry?)null);

            // Act
            var result = await _timeEntryService.DeleteTimeEntry(timeEntryId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.ErrorMessage.Should().Contain($"No Time Entry was found with ID {timeEntryId}.");

            _timeEntryRepoMock.Verify(r => r.GetTimeEntryById(timeEntryId), Times.Once);
        }

        [Fact]
        public async Task DeleteTimeEntry_ShouldReturnTimeEntry_WhenIdFound()
        {
            // Arrange
            var timeEntryId = 1;
            var employee = Emp();

            var expectedTimeEntry = new TimeEntry
            {
                TimeEntryId = timeEntryId,
                EmployeeId = employee.EmployeeId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                HoursWorked = 8
            };

            _timeEntryRepoMock
                .Setup(repo => repo.GetTimeEntryById(timeEntryId))
                .ReturnsAsync(expectedTimeEntry);

            // Act
            var result = await _timeEntryService.DeleteTimeEntry(timeEntryId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.TimeEntryId.Should().Be(timeEntryId);
            result.Data.EmployeeId.Should().Be(employee.EmployeeId);
            result.Data.Date.Should().Be(DateOnly.FromDateTime(DateTime.Now));
            result.Data.HoursWorked.Should().Be(8);

            _timeEntryRepoMock.Verify(r => r.GetTimeEntryById(timeEntryId), Times.Once);
        }
    }
}
