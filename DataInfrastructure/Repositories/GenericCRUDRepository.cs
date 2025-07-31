using Aviva.PaymentOrders.DataInfrastructure.Data;
using Aviva.PaymentOrders.Domain.Contracts;
using Aviva.PaymentOrders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Aviva.PaymentOrders.DataInfrastructure.Repositories
{


    public class GenericCRUDRepository<T> : ICRUDRepository<T> where T : Entity
    {
        // This is a simple in-memory storage for demonstration purposes.
        // In a real application, this would connect to a database.
        // protected List<T> data = new List<T>();

        private readonly InMemoryContext _context;
        private DbSet<T> dbSet;

        public GenericCRUDRepository(InMemoryContext context)
        {
            // Initialize the data list with some sample data if needed
            // This can be overridden in derived classes
            _context = context;
            this.dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // Simulating asynchronous operation
            // await Task.Delay(100); // Simulate some delay
            IQueryable<T> query = dbSet;
            query = query.Where(p => p != null && p.Id > 0); // Ensure we only return entities with valid IDs
            return await query.ToListAsync();
            // return await Task.Run(() => data.Where(p => p != null).Cast<T>());
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ValidationException("Invalid entity ID.");
                }
                IQueryable<T> query = dbSet;
                // query = query.Where(p => p.Id == id);
                var entity = await query.FirstOrDefaultAsync(p => p.Id == id);
                // if (entity == null)
                // {
                //     throw new KeyNotFoundException($"Entity with ID {id} not found.");
                // }
                return entity;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return a meaningful error message
                // You can log the exception or rethrow it as needed
                // For now, we will just throw a new exception with the message
                // return Task.FromResult(_mapper.Map<T>(entity));
                // return Task.FromResult(_mapper.Map<T>(entity));
                throw new Exception($"Internal server error: {ex.Message}");
            }

        }

        public async Task<T> AddAsync(T entity)
        {
            // data.Add(entity);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }
            // Add the entity to the DbSet
            dbSet.Add(entity);
            // Save changes to the context
            await _context.SaveChangesAsync();
            return entity;
        }
        
        public async Task UpdateAsync(T entity)
        {
            // Find the existing entity by ID
            IQueryable<T> query = dbSet;
            // Update the entity in the DbSet
            dbSet.Update(entity);
            // Save changes to the context
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(int id)
        {
           // Find the existing entity by ID
            IQueryable<T> query = dbSet;
            var existingEntity = await query.FirstOrDefaultAsync(p => p.Id == id);
            if (existingEntity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }
            // Remove the entity from the DbSet
            dbSet.Remove(existingEntity);
            // Save changes to the context
            await _context.SaveChangesAsync();
        }
    }
}