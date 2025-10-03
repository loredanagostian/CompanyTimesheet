using System.Net;

namespace Timesheet.API.Models
{
    public class ServiceResult<T>
    {
        public T? Data { get; private set; }
        public bool IsSuccess { get; private set; }
        public bool HasValidationErrors { get; private set; } = false;

        // General error info
        public string? ErrorMessage { get; private set; }
        
        // Validation-specific (field -> messages)
        public Dictionary<string, string[]>? ValidationErrors { get; private set; }

        // Success factory
        public static ServiceResult<T> Success(T data) => new ServiceResult<T>
        {
            Data = data,
            IsSuccess = true
        };

        // Failure factory
        public static ServiceResult<T> Failure(string errorMessage)
            => new ServiceResult<T>
            {
                ErrorMessage = errorMessage,
                IsSuccess = false
            };

        // Validation failure with multiple messages
        public static ServiceResult<T> ValidationFailure(
            Dictionary<string, string[]> errors,
            string? message = "One or more validation errors occurred.")
            => new ServiceResult<T>
            {
                IsSuccess = false,
                ErrorMessage = message,
                ValidationErrors = errors,
                HasValidationErrors = true
            };
    }
}