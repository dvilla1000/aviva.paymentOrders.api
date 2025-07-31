using System.ComponentModel.DataAnnotations.Schema;

namespace Aviva.PaymentOrders.Domain.Entities
{
    public class PaymentOrderDetail : Entity
    {
        public int PaymentOrderId { get; set; } // Foreign key to PaymentOrder
        public PaymentOrder PaymentOrder { get; set; } // Navigation property to PaymentOrder
        public int ProductId { get; set; } // Foreign key to Product
        [NotMapped]
        public string ProductName { get; set; } // Name of the product, can be used for display purposes
        public Product Product { get; set; } // Navigation property to Product entity
        public int Quantity { get; set; } // Quantity of the product in the order
        public double UnitPrice { get; set; } // Price per unit of the product        
    }
}