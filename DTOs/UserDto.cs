using Swashbuckle.AspNetCore.Annotations;

namespace Node_ApiService_Test.DTOs
{
    // DTO class for User
    public class UserDto
    {
        [SwaggerSchema(WriteOnly = true)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
