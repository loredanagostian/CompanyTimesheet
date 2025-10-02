using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Timesheet.API.DbContexts;
using Timesheet.API.Models;

namespace Timesheet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TimesheetContext _context;

        public AuthenticationController(IConfiguration configuration, TimesheetContext context)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public class AuthenticationRequestBody
        {
            public string? Email { get; set; }
            public string? Password { get; set; }
        }

        /// <summary>
        /// Authenticates a user using the provided email and password, and returns a JWT token if successful.
        /// </summary>
        /// <param name="authenticationRequestBody">The authentication request containing email and password.</param>
        /// <returns>A JWT token string if authentication is successful; otherwise, an error response.</returns>
        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate([FromBody] AuthenticationRequestBody authenticationRequestBody)
        {
            if (authenticationRequestBody is null || string.IsNullOrWhiteSpace(authenticationRequestBody.Email) || string.IsNullOrWhiteSpace(authenticationRequestBody.Password))
                return BadRequest("Email and password are required.");

            // Step 1: Validate the Email and Password
            var user = await ValidateUserCredentialsAsync(authenticationRequestBody.Email, authenticationRequestBody.Password);

            if (user is null)
                return Unauthorized("Invalid email or password.");

            // Step 2: Generate a JWT token
            var securityKey = GetSecurityKey(_configuration["Authentication:SecretForKey"]);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define the claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("employeeId", user.EmployeeId.ToString())
            };

            if (!string.IsNullOrWhiteSpace(user.FirstName))
                claims.Add(new Claim("given_name", user.FirstName!));

            if (!string.IsNullOrWhiteSpace(user.LastName))
                claims.Add(new Claim("family_name", user.LastName!));

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials
            );

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(tokenToReturn);
        }

        private SymmetricSecurityKey GetSecurityKey(string? secret)
        {
            if (string.IsNullOrWhiteSpace(secret))
                throw new InvalidOperationException("Authentication:SecretForKey is not configured.");

            // Accept either Base64 or plain text secrets
            byte[] keyBytes;
            try { keyBytes = Convert.FromBase64String(secret); }
            catch (FormatException) { keyBytes = Encoding.UTF8.GetBytes(secret); }

            return new SymmetricSecurityKey(keyBytes);
        }

        private async Task<AuthUser?> ValidateUserCredentialsAsync(string email, string password)
        {
            // Look up account by email (no tracking for auth)
            var account = await _context.UserAccounts
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.Email == email);

            if (account is null)
                return null;

            // TODO: replace with hashed password verification
            if (!string.Equals(account.Password, password, StringComparison.Ordinal))
                return null;

            string? firstName = null, lastName = null;

            // If you have an Employees table, load the names
            var employee = await _context.Employees
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.EmployeeId == account.EmployeeId);

            if (employee is not null)
            {
                firstName = employee.FirstName;
                lastName = employee.LastName;
            }

            return new AuthUser(account.Email, account.EmployeeId, firstName, lastName);
        }
    }
}