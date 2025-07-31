using Aviva.PaymentOrders.DataInfrastructure.Data;
using Aviva.PaymentOrders.Domain.Contracts;
using Aviva.PaymentOrders.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly InMemoryContext _context;
        private readonly ILogger<GenericCRUDRepository<T>> _logger;
        private DbSet<T> dbSet;

        public GenericCRUDRepository(InMemoryContext context, ILogger<GenericCRUDRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
            this.dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // Simulating asynchronous operation
            IQueryable<T> query = dbSet;
            query = query.Where(p => p != null && p.Id > 0); // Ensure we only return entities with valid IDs
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError($"Invalid entity ID: {id}");
                    throw new ValidationException("Invalid entity ID.");
                }
                IQueryable<T> query = dbSet;
                var entity = await query.FirstOrDefaultAsync(p => p.Id == id);                
                return entity;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, $"Entity with ID {id} not found.");
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving entity with ID {id}.");
                throw new Exception($"Internal server error: {ex.Message}");
            }

        }

        public async Task<T> AddAsync(T entity)
        {
            // data.Add(entity);
            if (entity == null)
            {
                _logger.LogError("Attempted to add a null entity.");
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }
            try
            {
                // Add the entity to the DbSet
                dbSet.Add(entity);
                // Save changes to the context
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Entity with ID {entity.Id} added successfully.");
                return entity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding entity to the database.");
                throw new Exception($"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new Exception($"Internal server error: {ex.Message}");
            }

        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                _logger.LogError("Attempted to update a null entity.");
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }
            // Validate the entity ID
            if (entity.Id <= 0)
            {
                _logger.LogError($"Invalid entity ID: {entity.Id}");
                throw new ValidationException("Invalid entity ID.");
            }
            try
            {
                // Find the existing entity by ID
                IQueryable<T> query = dbSet;
                // Update the entity in the DbSet
                dbSet.Update(entity);
                // Save changes to the context
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Entity with ID {entity.Id} updated successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding entity to the database.");
                throw new Exception($"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            // Validate the entity ID
            if (id <= 0)
            {
                _logger.LogError($"Invalid entity ID: {id}");
                throw new ValidationException("Invalid entity ID.");
            }
            try
            {
                // Check if the entity exists
                IQueryable<T> query = dbSet;
                var entity = await query.FirstOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    _logger.LogError($"Entity with ID {id} not found.");
                    throw new KeyNotFoundException($"Entity with ID {id} not found.");
                }
                // Remove the entity from the DbSet
                dbSet.Remove(entity);
                // Save changes to the context
                await _context.SaveChangesAsync();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, $"Entity with ID {id} not found.");
                throw new KeyNotFoundException(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding entity to the database.");
                throw new Exception($"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the entity.");
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }
    }
}