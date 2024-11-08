using Node_ApiService_Test.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Node_ApiService_Test.Controllers.ControllerExtensions
{
    public static class ControllerExtensions_Input
    {
        public class NotFoundWithProductsResponse
        {
            public string Warning { get; set; }
            public IEnumerable<ProductDto> Products { get; set; }
        }

        public class NotFoundWithUsersResponse
        {
            public string Warning { get; set; }
            public IEnumerable<UserDto> Users { get; set; }
        }


        public static ActionResult NotFoundWithProducts(this ControllerBase controller, IEnumerable<ProductDto> products)
        {
            var response = new NotFoundWithProductsResponse
            {
                Warning = "Product not found. Please pick someone from this list.",
                Products = products
            };
            return controller.StatusCode(404, response);
        }

        public static ActionResult NotFoundWithUsers(this ControllerBase controller, IEnumerable<UserDto> products)
        {
            var response = new NotFoundWithUsersResponse
            {
                Warning = "User not found. Please pick someone from this list.",
                Users = products
            };
            return controller.StatusCode(404, response);
        }
    }
}

