using Aviva.PaymentOrders.Domain.Entities;

namespace Aviva.PaymentOrders.Domain.Entities
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}