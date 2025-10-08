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
    public class UserAccountServiceTests
    {
        private readonly Mock<IEmployeeService> _employeeServiceMock;
        private readonly Mock<IUserAccountRepository> _userAccountRepoMock;
        private readonly UserAccountService _userAccountService;

        public UserAccountServiceTests()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _userAccountRepoMock = new Mock<IUserAccountRepository>();
            _userAccountService = new UserAccountService(_employeeServiceMock.Object, _userAccountRepoMock.Object);
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

        // GET User Accounts
        [Fact]
        public async Task GetUserAccounts_ShouldReturnAll_WhenRepositoryReturnsData()
        {
            // Arrange
            var list = new List<UserAccount> { new() { UserAccountId = 10, Email = "a@b.com" } };
            _userAccountRepoMock
                .Setup(r => r.GetUserAccounts())
                .ReturnsAsync(ServiceResult<IEnumerable<UserAccount>>.Success(list));

            // Act
            var result = await _userAccountService.GetUserAccounts();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Should().HaveCount(1);

            _userAccountRepoMock.Verify(r => r.GetUserAccounts(), Times.Once);
        }

        [Fact]
        public async Task GetUserAccounts_ShouldReturnEmpty_WhenRepositoryIsEmpty()
        {
            // Arrange
            _userAccountRepoMock
                .Setup(r => r.GetUserAccounts())
                .ReturnsAsync(ServiceResult<IEnumerable<UserAccount>>.Success(Enumerable.Empty<UserAccount>()));

            // Act
            var result = await _userAccountService.GetUserAccounts();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Should().BeEmpty();

            _userAccountRepoMock.Verify(r => r.GetUserAccounts(), Times.Once);
        }

        // CREATE User Account
        [Fact]
        public async Task CreateUserAccount_ShouldReturnFailure_WhenEmployeeNotFound()
        {
            // Arrange
            int empId = 42;

            _employeeServiceMock
                .Setup(s => s.GetEmployeeById(empId))
                .ReturnsAsync(ServiceResult<Employee>.Failure("Not found"));

            var dto = new CreateUserAccountDto
            {
                EmployeeId = empId
            };

            // Act
            var result = await _userAccountService.CreateUserAccount(dto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain($"No Employee was found with ID {empId}");

            _userAccountRepoMock.Verify(r => r.CreateUserAccount(It.IsAny<UserAccount>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserAccount_ShouldReturnValidationError_WhenValidationFails()
        {
            // Arrange
            int empId = 1;
            var employee = Emp(id: empId);

            _employeeServiceMock
                .Setup(s => s.GetEmployeeById(empId))
                .ReturnsAsync(ServiceResult<Employee>.Success(employee));

            var dto = new CreateUserAccountDto
            {
                EmployeeId = empId,
                Email = "invalid-email",
                Password = "short"
            };

            // Act
            var result = await _userAccountService.CreateUserAccount(dto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.HasValidationErrors.Should().BeTrue();
            result.ValidationErrors.Should().ContainKey("email");
            result.ValidationErrors.Should().ContainKey("password");

            _userAccountRepoMock.Verify(r => r.CreateUserAccount(It.IsAny<UserAccount>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserAccount_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            // Arrange
            int empId = 1;
            var existingEmail = "ana.ionescu@company.com";
            var employee = new Employee
            {
                EmployeeId = empId,
                FirstName = "Ana",
                LastName = "Ionescu",
                ContractType = ContractType.FullTime,
                UserAccounts = new List<UserAccount>
                {
                    new() { Email = existingEmail } // already has this email
                }
            };

            _employeeServiceMock
                .Setup(s => s.GetEmployeeById(empId))
                .ReturnsAsync(ServiceResult<Employee>.Success(employee));

            var dto = new CreateUserAccountDto
            {
                EmployeeId = empId,
                Email = existingEmail // trying to create with same email
            };

            // Act
            var result = await _userAccountService.CreateUserAccount(dto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.ErrorMessage.Should().Contain($"An User Account already exists for Employee with ID {empId} and Email {dto.Email}");

            _userAccountRepoMock.Verify(r => r.CreateUserAccount(It.IsAny<UserAccount>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserAccount_ShouldCreateAccount_WhenValidInput()
        {
            // Arrange
            int empId = 1;
            var employee = Emp(id: empId);

            _employeeServiceMock
                .Setup(s => s.GetEmployeeById(empId))
                .ReturnsAsync(ServiceResult<Employee>.Success(employee));

            var dto = new CreateUserAccountDto
            {
                EmployeeId = empId
            };

            UserAccount? createdAccount = null;

            _userAccountRepoMock
                .Setup(r => r.CreateUserAccount(It.IsAny<UserAccount>()))
                .Callback<UserAccount>(ua => createdAccount = ua)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userAccountService.CreateUserAccount(dto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Email.Should().Be(dto.Email ?? $"{employee.FirstName.ToLower()}.{employee.LastName.ToLower()}@company.com");
            result.Data.EmployeeId.Should().Be(empId);
            result.Data.Password.Should().Be(dto.Password ?? "P4$$W0Rd");

            if (dto.Password is null)
                result.Data.HasDefaultPassword.Should().BeTrue();
            else
                result.Data.HasDefaultPassword.Should().BeFalse();

            if (dto.Email is null)
                result.Data.IsAlias.Should().BeFalse();
            else
                result.Data.IsAlias.Should().BeTrue();

            // Verify that repository was called once
            _userAccountRepoMock.Verify(r => r.CreateUserAccount(It.IsAny<UserAccount>()), Times.Once);

            // Verify that the created account was passed correctly
            createdAccount.Should().NotBeNull();
            createdAccount.Email.Should().Be(dto.Email ?? $"{employee.FirstName.ToLower()}.{employee.LastName.ToLower()}@company.com");
            createdAccount.EmployeeId.Should().Be(empId);
        }

        // GET User Account by ID
        [Fact]
        public async Task GetUserAccountById_ShouldReturnFailure_WhenIdNotFound()
        {
            // Arrange
            int userAccountId = 1;
            _userAccountRepoMock
                .Setup(r => r.GetUserAccountById(userAccountId))
                .ReturnsAsync((UserAccount?)null);

            // Act
            var result = await _userAccountService.GetUserAccountById(userAccountId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();
            result.ErrorMessage.Should().Contain($"No User Account was found with ID {userAccountId}");

            _userAccountRepoMock.Verify(r => r.GetUserAccountById(userAccountId), Times.Once);
        }

        [Fact]
        public async Task GetUserAccountById_ShouldReturnUserAccount_WhenIdFound()
        {
            // Arrange
            var emp = Emp();
            var accountId = 42;
            var expectedEmail = $"{emp.FirstName.ToLower()}.{emp.LastName.ToLower()}@company.com";

            var expectedAccount = new UserAccount
            {
                UserAccountId = accountId,
                EmployeeId = emp.EmployeeId,
                Email = expectedEmail,
                HasDefaultPassword = true,
                IsAlias = false
            };

            _userAccountRepoMock
                .Setup(r => r.GetUserAccountById(accountId))
                .ReturnsAsync(expectedAccount);

            // Act
            var result = await _userAccountService.GetUserAccountById(accountId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data.UserAccountId.Should().Be(accountId);
            result.Data.EmployeeId.Should().Be(emp.EmployeeId);
            result.Data.Email.Should().Be(expectedEmail);

            _userAccountRepoMock.Verify(r => r.GetUserAccountById(accountId), Times.Once);
        }

        // DELETE User Account
        [Fact]
        public async Task DeleteUserAccount_ShouldReturnFailure_WhenIdNotFound()
        {
            // Arrange
            var accountId = 42;

            _userAccountRepoMock
                .Setup(r => r.GetUserAccountById(accountId))
                .ReturnsAsync((UserAccount?)null);

            // Act
            var result = await _userAccountService.DeleteUserAccount(accountId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain($"No User Account was found with ID {accountId}");

            _userAccountRepoMock.Verify(r => r.GetUserAccountById(accountId), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAccount_ShouldDeleteAccount_WhenIdFound()
        {
            // Arrange
            var emp = Emp();
            var accountId = 42;
            var expectedEmail = $"{emp.FirstName.ToLower()}.{emp.LastName.ToLower()}@company.com";

            var existingAccount = new UserAccount
            {
                UserAccountId = accountId,
                EmployeeId = emp.EmployeeId,
                Email = expectedEmail,
                HasDefaultPassword = true,
                IsAlias = false
            };

            _userAccountRepoMock
                .Setup(r => r.GetUserAccountById(accountId))
                .ReturnsAsync(existingAccount);

            // Act
            var result = await _userAccountService.DeleteUserAccount(accountId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.UserAccountId.Should().Be(accountId);
            result.Data.EmployeeId.Should().Be(emp.EmployeeId);
            result.Data.Email.Should().Be(expectedEmail);

            _userAccountRepoMock.Verify(r => r.GetUserAccountById(accountId), Times.Once);
        }
    }
}
