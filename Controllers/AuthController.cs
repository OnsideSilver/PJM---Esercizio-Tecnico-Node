using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Node_ApiService_Test___Esercizio_Tecnico_Node.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Node_ApiService_Test___Esercizio_Tecnico_Node.Controllers
{
    //username and password = "string"
    //You can execute the command without modifying anything to recieve the token.
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        // Inject the JWT Service
        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Entities.LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return Unauthorized();
            }
            // Hardcoded username and password check
            else if (model.Username == "string" && model.Password == "string")
            {

                var token = _jwtService.GenerateJwtToken(model.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password.");
        }
    }
}
