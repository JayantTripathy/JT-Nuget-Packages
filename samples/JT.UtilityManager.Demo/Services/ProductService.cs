using JT.UtilityManager.Demo.Models;

namespace JT.UtilityManager.Demo.Services
{
    public class ProductService
    {
        public Product GetProduct(int id) => new Product
        {
            Id = id,
            Name = $"Product {id}",
            Price = id * 10
        };
    }
}
