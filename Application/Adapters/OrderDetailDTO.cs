namespace Aviva.PaymentOrders.Application.Adapters
{
    // OrderDetailDTO is a Data Transfer Object (DTO) for the PaymentOrderDetail entity.
    // It is used to transfer order detail data between different layers of the application.
    public class OrderDetailDTO
    {
        public int Id { get; set; } // Unique identifier for the order detail
        public int PaymentOrderId { get; set; } // Foreign key to PaymentOrder
        public int ProductId { get; set; } // Foreign key to Product
        public int Quantity { get; set; } // Quantity of the product in the order
        public decimal UnitPrice { get; set; } // Price per unit of the product
        public string ProductName { get; set; } // Name of the product
        public string ProductDescription { get; set; } // Description of the product
    }
}   