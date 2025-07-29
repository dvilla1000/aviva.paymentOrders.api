namespace Aviva.PaymentOrders.Domain.Entities
{
    public class PaymentOrderDetail : Entity
    {
        public int PaymentOrderId { get; set; } // Foreign key to PaymentOrder
        public PaymentOrder PaymentOrder { get; set; } // Navigation property to PaymentOrder
        public int ProductId { get; set; } // Foreign key to Product
        public Product Product { get; set; } // Navigation property to Product entity
        public int Quantity { get; set; } // Quantity of the product in the order
        public decimal UnitPrice { get; set; } // Price per unit of the product        
    }
}