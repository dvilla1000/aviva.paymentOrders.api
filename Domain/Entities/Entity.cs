using Aviva.PaymentOrders.Domain.Contracts;

namespace Aviva.PaymentOrders.Domain.Entities
{
    public abstract class Entity : IEntity
    {
        public Entity()
        {
            CreatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}