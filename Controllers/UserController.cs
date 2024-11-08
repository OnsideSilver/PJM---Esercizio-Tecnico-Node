using Node_ApiService_Test.Controllers.ControllerExtensions;
using Node_ApiService_Test.DTOs;
using Node_ApiService_Test.Services;
using Microsoft.AspNetCore.Mvc;

namespace Node_ApiService_Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        // Constructor injection
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // GET all users
        [HttpGet("all")]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            var user = _userService.GetAllUsers(); // Get all users from the service
            return Ok(user);
        }

        // GET an user
        [HttpGet]
        public ActionResult<UserDto> GetUser([FromQuery] Guid? id, [FromQuery] string? name)
        {
            UserDto user = null;

            // If both ID and Name are provided, prioritize ID
            if (id.HasValue)
            {
                user = _userService.ReadId(id.Value);
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                user = _userService.ReadName(name);
            }

            if (user == null)
            {
                return this.NotFoundWithUsers(_userService.GetAllUsers());
            }

            return Ok(user);
        }

        // POST a new user
        [HttpPost("{name}/{email}")]
        public ActionResult<UserDto> CreateUser(string name, string email)
        {
            var userDto = new UserDto
            {
                Name = name,
                Email = email
            };

            var createdUser = _userService.Create(userDto);
            if (createdUser != null) 
            {
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            return NotFound("The email format is invalid. Please use the following format: example@domain.com");
        }

        // PUT an existing User
        [HttpPut("{id}")]
        public ActionResult UpdateUser(Guid id, UserDto userDto)
        {
            var message = "Something went wrong. Verify if the ID is present in this list and the mail follows this format: example@domain.com.";
            var user = _userService.Update(id, userDto);
            if (user == null)
            {
                return NotFound(new { message, users = _userService.GetAllUsers() });
            }
            return Ok(user);
        }

        // DELETE an user
        [HttpDelete]
        public ActionResult DeleteUser([FromQuery] Guid? id, [FromQuery] string? name)
        {
            // If both ID and Name are provided, prioritize ID
            if (!id.HasValue && string.IsNullOrEmpty(name))
            {
                return this.NotFoundWithUsers(_userService.GetAllUsers());
            }

            else if (id.HasValue && !string.IsNullOrEmpty(name) || id.HasValue && string.IsNullOrEmpty(name))
            {
                bool deleted = _userService.DeleteId(id.Value);
                if (!deleted)
                {
                    return this.NotFoundWithUsers(_userService.GetAllUsers());
                }
                return Ok(new { message = "User successfully deleted. N.B: in the case you have filled both Id and Name of 2 different users, only the user associated with the id has been deleted." });
            }

            else if (!string.IsNullOrEmpty(name))
            {
                bool deleted = _userService.DeleteName(name);
                if (!deleted)
                {
                    return this.NotFoundWithUsers(_userService.GetAllUsers());
                }
                return Ok(new { message = "User successfully deleted." });
            }
            return BadRequest("Invalid request.");
        }
    }
}
