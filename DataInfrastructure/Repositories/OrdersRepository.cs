using Aviva.PaymentOrders.DataInfrastructure.Data;
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
        public OrdersRepository(InMemoryContext context) : base(context)
        {
            // Initialize the orders array with some sample data
            // This can be overridden in derived classes
            /*
            for (int i = 0; i < 100; i++)
            {
                var order = new PaymentOrder
                {
                    Id = i + 1,
                    PaymentMethod = i % 2 == 0 ? "Cash" : "Card",
                    Status = i % 3 == 0 ? "Pending" : (i % 3 == 1 ? "Completed" : "Cancelled"),
                    OrderIdProvider = i + 1000, // Example OrderId from Provider API
                    ProviderRef = i % 3 == 0 ? "PagaFacil" : (i % 3 == 1 ? "CazaPagos" : "Transferencia"), // Example provider reference
                    Products = new List<PaymentOrderDetail>()
                        {
                            new PaymentOrderDetail() { Id = 1, UnitPrice=1000, Quantity=2, ProductId=1 },
                            new PaymentOrderDetail() { Id = 2, UnitPrice=2000, Quantity=1, ProductId=2 },
                            new PaymentOrderDetail() { Id = 3, UnitPrice=6000, Quantity=3, ProductId=3 }
                        }
                };
                data.Add(order);
            }
            */
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