using Aviva.PaymentOrders.Domain.Contracts;
using Aviva.PaymentOrders.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aviva.PaymentOrders.DataInfrastructure.Repositories
{

    public class OrdersRepository : GenericCRUDRepository<PaymentOrder>, ICRUDRepository<PaymentOrder>
    {
        // This is a simple in-memory storage for demonstration purposes.
        // In a real application, this would connect to a database.
        public OrdersRepository()
        {
            // Initialize the products array with some sample data
            for (int i = 0; i < 100; i++)
            {
                var order = new PaymentOrder
                {
                    Id = i + 1,
                    PaymentMethod = i % 2 == 0 ? "Cash" : "Card",
                    Status = i % 3 == 0 ? "Pending" : (i % 3 == 1 ? "Completed" : "Cancelled"),
                    OrderIdProvider = i + 1000, // Example OrderId from Provider API
                    ProviderRef = i % 3 == 0 ? "PagaFacil" : (i % 3 == 1 ? "CazaPagos" : "Transferencia"), // Example provider reference
                    Products = new List<Product>()
                        {
                            new Product() { Id = 1, Name=$"Product  1", UnitPrice=1000, Description="Description for Product 1", Status="Available" },
                            new Product() { Id = 2, Name=$"Product  2", UnitPrice=2000, Description="Description for Product 2", Status="Available" },
                            new Product() { Id = 3, Name=$"Product  3", UnitPrice=6000, Description="Description for Product 3", Status="Available" }
                        }// Assuming Product is a valid entity
                    // Name = $"Product {i + 1}",
                    // UnitPrice = (decimal)(10 + i * 5),
                    // Description = $"Description for Product {i + 1}",
                    // Status = i % 2 == 0 ? "Available" : "Not Available",
                };
                data.Add(order);
            }

        }

        public async Task CancelOrderAsync(int id)
        {
            // Logic to cancel an order by id
            var order = await GetByIdAsync(id);
            if (order != null)
            {
                order.Status = "Cancelled"; // Update the status to "Cancelled"
                await UpdateAsync(order); // Save the changes
            }
            else
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }
        }
    }
}