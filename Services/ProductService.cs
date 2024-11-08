using Node_ApiService_Test.DTOs;
using Node_ApiService_Test.Entities;
using Node_ApiService_Test.Utilities;
using System.Transactions;

namespace Node_ApiService_Test.Services
{
    public class ProductService : CrudServiceBase<Product, ProductDto>
    {
        private readonly List<Product> _productList;
        private readonly ILogger<ProductService> _logger;

        // Constructor injection of the product list and logger
        public ProductService(ILogger<ProductService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _productList = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product A", Price = 10.99m },
                new Product { Id = Guid.NewGuid(), Name = "Product B", Price = 15.49m },
                new Product { Id = Guid.NewGuid(), Name = "Product C", Price = 12.38m },
                new Product { Id = Guid.NewGuid(), Name = "Product D", Price = 60.00m },
                new Product { Id = Guid.NewGuid(), Name = "Product E", Price = 33.99m }
            };
        }

        // Return all products
        public IEnumerable<ProductDto> GetAllProducts()
        {
            try
            {
                return _productList.Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all products");
                return Enumerable.Empty<ProductDto>();
            }
        }

        // Create a new product
        public override ProductDto Create(ProductDto dto)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    dto.Name = SecurityChecks.SanitizeString(dto.Name); // Sanitize the string
                    var product = MapToEntity(dto);
                    product.Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id;

                    _productList.Add(product);

                    scope.Complete(); // Commit transaction
                    return MapToDto(product);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating product, rolling back transaction");
                    return null;
                }
            }
        }

        // Read a product by ID
        public override ProductDto ReadId(Guid id)
        {
            try
            {
                var product = _productList.FirstOrDefault(p => p.Id == id);
                return MapToDto(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product with ID {id}");
                return null;
            }
        }

        // Read a product by Name
        public override ProductDto ReadName(string name)
        {
            try
            {
                name = SecurityChecks.SanitizeString(name); // Sanitize the string
                var product = _productList.FirstOrDefault(p => p.Name != null && p.Name.ToLower().Contains(name.ToLower())); //Search by like
                return MapToDto(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product with Name {name}");
                return null;
            }
        }

        // Update a product
        public override ProductDto Update(Guid id, ProductDto dto)
        {
            try
            {
                dto.Name = SecurityChecks.SanitizeString(dto.Name); // Sanitize the string
                var product = _productList.FirstOrDefault(p => p.Id == id);
                if (product == null) return null;

                product.Name = dto.Name;
                product.Price = dto.Price;
                return MapToDto(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product with ID {id}");
                return null;
            }
        }

        // Delete a product by ID
        public override bool DeleteId(Guid id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var product = _productList.FirstOrDefault(p => p.Id == id);
                    if (product == null) return false;

                    _productList.Remove(product);

                    scope.Complete(); // Commit transaction
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error deleting product with ID {id}, rolling back transaction");
                    return false;
                }
            }
        }

        // Delete a product by Name
        public override bool DeleteName(string name)
        {
            using (var scope = new TransactionScope())
                try
                {
                    name = SecurityChecks.SanitizeString(name); // Sanitize the string
                    var product = _productList.FirstOrDefault(p => p.Name == name);
                    if (product == null) return false;

                    _productList.Remove(product);
                    scope.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error deleting product with Name {name}");
                    return false;
                }
        }

        // Map DTO to Entity
        protected override Product MapToEntity(ProductDto dto)
        {
            try
            {
                return new Product
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Price = dto.Price
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping DTO to entity");
                return null;
            }
        }

        // Map Entity to DTO
        protected override ProductDto MapToDto(Product entity)
        {
            try
            {
                return new ProductDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Price = entity.Price
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
