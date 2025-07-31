using System.ComponentModel.DataAnnotations.Schema;

namespace Aviva.PaymentOrders.Domain.Entities
{
    public class PaymentOrderExternal
    {
        public string OrderId { get; set; } // Unique identifier for the payment order
        public double Amount { get; set; } // Total amount for the payment order        
        public string Method { get; set; }
        public string Status { get; set; } // Status of the order, e.g., "Pending", "Completed", "Cancelled"

        public ICollection<Fee> Fees { get; set; } // Navigation property to Fees entity
        public ICollection<ProductExternal> Products { get; set; } // Navigation property to ProductExternal entity
    }
}