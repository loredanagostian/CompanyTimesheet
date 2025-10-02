
namespace Timesheet.API.Models.DTOs
{
    public class CreateEmployeeDto
    {
        public string CNP { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public ContractType ContractType { get; set; }
    }
}
