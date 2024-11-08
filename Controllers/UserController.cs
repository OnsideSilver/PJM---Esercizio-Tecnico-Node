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

        // GET All Users
        [HttpGet("all")]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            var user = _userService.GetAllUsers();
            return Ok(user); // Return the list of users
        }

        // GET an User
        [HttpGet]
        public ActionResult<UserDto> GetUser([FromQuery] Guid? id, [FromQuery] string name)
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
                return this.NotFoundWithUsers(_userService.GetAllUsers()); // Return list of products if none is found
            }

            return Ok(user);
        }

        // POST a new User
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
            var user = _userService.Update(id, userDto);
            if (user == null)
            {
                return this.NotFoundWithUsers(_userService.GetAllUsers());
            }
            return Ok(_userService.GetAllUsers());
        }

        // DELETE an User
        [HttpDelete]
        public ActionResult DeleteUser([FromQuery] Guid? id, [FromQuery] string? name)
        {
            if (!id.HasValue && string.IsNullOrEmpty(name))
            {
                return BadRequest("Provide either an Id or a Name.");
            }

            else if (id.HasValue && !string.IsNullOrEmpty(name) || id.HasValue && string.IsNullOrEmpty(name))
            {
                bool deleted = _userService.DeleteId(id.Value);
                if (!deleted)
                {
                    return NotFound("User could not be deleted by Id.");
                }
                return Ok(new { message = "User successfully deleted. N.B: in the case you have filled both Id and Name of 2 different users, only the user associated with the id has been deleted." });
            }

            else if (!string.IsNullOrEmpty(name))
            {
                bool deleted = _userService.DeleteName(name);
                if (!deleted)
                {
                    return NotFound("User could not be deleted by name.");
                }
                return Ok(new { message = "User successfully deleted." });
            }
            return BadRequest("Invalid request.");
        }
    }
}
