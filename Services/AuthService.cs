using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Node_ApiService_Test___Esercizio_Tecnico_Node.Services
{

    public interface IJwtService
    {
        string GenerateJwtToken(string username);
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string username)
        {
            try
            {
                // Generate the claims
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                // Get the key from the configuration
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                // Create signing credentials
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // Create the JWT token
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                // Return the token as a string
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // Log the error 
                throw new InvalidOperationException("An error occurred while generating the JWT token.", ex);
            }
        }
    }
}

