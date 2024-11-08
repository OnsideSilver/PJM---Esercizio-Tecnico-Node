using Node_ApiService_Test.DTOs;
using Node_ApiService_Test.Entities;
using Node_ApiService_Test.Utilities;
using System.Transactions;

namespace Node_ApiService_Test.Services
{
    public class UserService : CrudServiceBase<User, UserDto>
    {
        private readonly List<User> _userList;
        private readonly ILogger<UserService> _logger;

        // Constructor injection of the product list and logger
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _userList = new List<User>
            {
            new User { Id = Guid.NewGuid(), Name = "Mario Rossi", Email = "Mario@Mario.com" },
            new User { Id = Guid.NewGuid(), Name = "Alessio Bianchi", Email = "Alessio@Alessio.com" },
            new User { Id = Guid.NewGuid(), Name = "Anna Viola", Email = "Anna@Anna.com" },
            new User { Id = Guid.NewGuid(), Name = "Alvaro Verdi", Email = "Alvaro@Alvaro.com" },
            new User { Id = Guid.NewGuid(), Name = "Pietro Rossi", Email = "Pietro@Pietro.com" }
            };
        }

        // Return all users
        public IEnumerable<UserDto> GetAllUsers()
        {
            try
            {
                return _userList.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all Users");
                return Enumerable.Empty<UserDto>();
            }
        }

        // Create a new user with emulated transaction
        public override UserDto Create(UserDto dto)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    dto.Name = SecurityChecks.SanitizeString(dto.Name); // Sanitize the string
                    dto.Email = SecurityChecks.SanitizeAndValidateEmail(dto.Email); // Sanitize and validate the email

                    var user = MapToEntity(dto);
                    user.Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id;

                    _userList.Add(user);

                    scope.Complete(); // Commit transaction
                    return MapToDto(user);
                }
                catch (FormatException ex)
                {
                    _logger.LogError(ex, "Invalid email format");
                    return null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating user, rolling back transaction");
                    return null;
                }
            }
        }

        // Read an user by ID
        public override UserDto ReadId(Guid id)
        {
            try
            {
                var user = _userList.FirstOrDefault(p => p.Id == id);
                return MapToDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with ID {id}");
                return null;
            }
        }

        // Read an user by Name
        public override UserDto ReadName(string name)
        {
            try
            {
                name = SecurityChecks.SanitizeString(name); // Sanitize the string
                var user = _userList.FirstOrDefault(p => p.Name != null && p.Name.ToLower().Contains(name.ToLower())); //Search by like

                return MapToDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with Name {name}");
                return null;
            }
        }

        // Update an user
        public override UserDto Update(Guid id, UserDto dto)
        {
            try
            {
                dto.Name = SecurityChecks.SanitizeString(dto.Name);
                dto.Email = SecurityChecks.SanitizeAndValidateEmail(dto.Email);

                var user = _userList.FirstOrDefault(p => p.Id == id);
                if (user == null) return null;

                user.Name = dto.Name;
                user.Email = dto.Email;
                return MapToDto(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {id}");
                return null;
            }
        }

        // Delete an user by ID
        public override bool DeleteId(Guid id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var user = _userList.FirstOrDefault(p => p.Id == id);
                    if (user == null) return false;

                    _userList.Remove(user);

                    scope.Complete(); // Commit transaction
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error deleting user with ID {id}, rolling back transaction");
                    return false;
                }
            }
        }

        // Delete an user by Name
        public override bool DeleteName(string name)
        {
            using (var scope = new TransactionScope())
                try
                {
                    name = SecurityChecks.SanitizeString(name); // Sanitize the string
                    var user = _userList.FirstOrDefault(p => p.Name == name);
                    if (user == null) return false;

                    _userList.Remove(user);
                    scope.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error deleting user with Name {name}");
                    return false;
                }
        }

        // Map DTO to Entity
        protected override User MapToEntity(UserDto dto)
        {
            try
            {
                return new User
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Email = dto.Email
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping DTO to entity");
                return null;
            }
        }

        // Map Entity to DTO
        protected override UserDto MapToDto(User entity)
        {
            try
            {
                return new UserDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Email = entity.Email
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping entity to DTO");
                return null;
            }
        }
    }
}
