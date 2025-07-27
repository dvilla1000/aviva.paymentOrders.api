namespace Aviva.PaymentOrders.Application.Adapters
{
    // OrderDTO is a Data Transfer Object (DTO) for the PaymentOrder entity.
    // It is used to transfer order data between different layers of the application.}
    public class OrderDTO
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; } // Status of the order, e.g., "Pending", "Completed", "Cancelled"
        public List<ProductDTO> Products { get; set; } // Navigation property to ProductDTO
    }
}