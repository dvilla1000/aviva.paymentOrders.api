namespace Aviva.PaymentOrders.Application.Adapters
{
    // ProductDTO is a Data Transfer Object (DTO) for the Product entity.
    // It is used to transfer product data between different layers of the application.
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}