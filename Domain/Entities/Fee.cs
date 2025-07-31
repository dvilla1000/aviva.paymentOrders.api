namespace Aviva.PaymentOrders.Domain.Entities
{
    public class Fee
    {
        public string Name { get; set; } // Name of the product
        public double Amount { get; set; } // Price per unit of the product
    }
}