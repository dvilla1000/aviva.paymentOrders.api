using Aviva.PaymentOrders.Domain.Contracts;
using Aviva.PaymentOrders.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aviva.PaymentOrders.DataInfrastructure.Repositories
{

    public class ProductsRepository : GenericCRUDRepository<Product>, ICRUDRepository<Product>
    {
        // This is a simple in-memory storage for demonstration purposes.
        // In a real application, this would connect to a database.
        public ProductsRepository()
        {
            // Initialize the products array with some sample data
            for (int i = 0; i < 100; i++)
            {
                var product = new Product
                {
                    Id = i + 1,
                    Name = $"Product {i + 1}",
                    UnitPrice = (decimal)(10 + i * 5),
                    Description = $"Description for Product {i + 1}",
                    Status = i % 2 == 0 ? "Available" : "Not Available",

                };
                data.Add(product);
            }

        }
    }
}