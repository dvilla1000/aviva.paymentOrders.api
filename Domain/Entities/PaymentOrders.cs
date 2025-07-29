using System.ComponentModel.DataAnnotations.Schema;

namespace Aviva.PaymentOrders.Domain.Entities
{
    public class PaymentOrder : Entity
    {
        public string PaymentMethod { get; set; }
        public string Status { get; set; } // Status of the order, e.g., "Pending", "Completed", "Cancelled"
        public int OrderIdProvider { get; set; } // OrderId from Provider API
        public string ProviderRef { get; set; } // Provider is a string representing the payment provider
        public ICollection<PaymentOrderDetail> PaymentOrderDetails { get; set; } // Navigation property to PaymentOrderDetail entity
    }
}