using Aviva.PaymentOrders.Domain.Contracts;
using Aviva.PaymentOrders.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Aviva.PaymentOrders.DataInfrastructure.Repositories
{


    public class GenericCRUDRepository<T> : ICRUDRepository<T> where T : Entity
    {
        // This is a simple in-memory storage for demonstration purposes.
        // In a real application, this would connect to a database.
        protected List<T> data = new List<T>();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // Simulating asynchronous operation
            await Task.Delay(100); // Simulate some delay
            return await Task.Run(() => data.Where(p => p != null).Cast<T>());
        }

        public async Task<T> GetByIdAsync(int id)
        {
            // Simulating asynchronous operation
            await Task.Delay(100); // Simulate some delay
            // Use Task.Run to offload the search to a separate thread
            var entity = await Task.Run(() => data.FirstOrDefault(p => p.Id == id));
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }
            return entity;
        }

        public async Task AddAsync(T entity)
        {
            data.Add(entity);
            // Simulating asynchronous operation
            await Task.CompletedTask;
            // return Task.CompletedTask;
        }
        public Task UpdateAsync(T entity) 
        {
            // Find the existing entity by ID
            var existingEntity = data.FirstOrDefault(p => p.Id == entity.Id);
            if (existingEntity != null)
            {
                // Update the existing entity's properties
                existingEntity = entity;
            }
            else
            {
                throw new KeyNotFoundException($"Entity with ID {entity.Id} not found.");
            }
            // Simulating asynchronous operation
            return Task.CompletedTask;
        }
        
        public async Task DeleteAsync(int id)
        {
            // var entity = data.FirstOrDefault(p => p.Id == id);
            var entity = await Task.Run(() => data.FirstOrDefault(p => p.Id == id));
            if (entity != null)
            {
                data.Remove(entity);
            }
            await Task.CompletedTask;
        }
    }
}