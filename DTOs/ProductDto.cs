using Swashbuckle.AspNetCore.Annotations;

namespace Node_ApiService_Test.DTOs
{
    // DTO class for Product
    public class ProductDto
    {
        [SwaggerSchema(WriteOnly = true)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
