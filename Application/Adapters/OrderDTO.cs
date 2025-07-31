namespace Aviva.PaymentOrders.Application.Adapters
{
    // OrderDTO is a Data Transfer Object (DTO) for the PaymentOrder entity.
    // It is used to transfer order data between different layers of the application.}
    public class OrderDTO
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }
        public double Amount { get; set; } // Amount of the payment order
        public string? Status { get; set; } // Status of the order, e.g., "Pending", "Completed", "Cancelled"
        public string? OrderIdProvider { get; set; } // OrderId from Provider API
        public string? ProviderRef { get; set; } // // Response from the payment provider
        public string? ProviderName { get; set; } // Name of the payment provider
        public ICollection<OrderDetailDTO> Products { get; set; } // Navigation property to OrderDetailDTO
    }
}