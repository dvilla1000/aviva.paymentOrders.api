using Aviva.PaymentOrders.Domain.Contracts;
using Aviva.PaymentOrders.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aviva.PaymentOrders.DataInfrastructure.Repositories
{


    public class GenericCRUDRepository<T> : ICRUDRepository<T> where T : Entity
    {
        // This is a simple in-memory storage for demonstration purposes.
        // In a real application, this would connect to a database.
        protected List<T> data = new List<T>();

        public Task<IEnumerable<T>> GetAllAsync() => Task.FromResult<IEnumerable<T>>(data.Where(p => p != null).Cast<T>());

        public Task<T> GetByIdAsync(int id) => Task.FromResult(data.FirstOrDefault(p => p.Id == id));

        public Task AddAsync(T entity)
        {
            data.Add(entity);
            return Task.CompletedTask;
        }
        public Task UpdateAsync(T entity) => throw new NotImplementedException();
        public Task DeleteAsync(int id) 
        {
            var entity = data.FirstOrDefault(p => p.Id == id);
            if (entity != null)
            {
                data.Remove(entity);
            }
            return Task.CompletedTask;
        }
    }
}