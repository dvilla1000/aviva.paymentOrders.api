using Aviva.PaymentOrders.Domain.Contracts;
using Aviva.PaymentOrders.Domain.Entities;
using Aviva.PaymentOrders.DataInfrastructure.Repositories;
using Aviva.PaymentOrders.Application.Adapters;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Aviva.PaymentOrders.Application.Services
{

    // ProductService is a service class that provides methods to interact with Product entities.
    public class ProductService
    {
        private readonly ProductsRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(ProductsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        // GetAllProductsAsync retrieves all products and maps them to a list of ProductDTO.
        // It returns an IEnumerable of ProductDTO.
        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync() {
            try
            {
                // Retrieve all products from the repository
                var products = await _repository.GetAllAsync();
                return _mapper.Map<List<ProductDTO>>(products);
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        // GetProductByIdAsync retrieves a product by its ID and maps it to a ProductDTO.
        // If the product is not found, it returns null.
        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("Invalid product ID.");
            try
            {
                var product = await _repository.GetByIdAsync(id);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {id} not found.");
                }
                return _mapper.Map<ProductDTO>(product);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }

        // AddProductAsync maps the ProductDTO to a Product entity and adds it to the repository.
        public async Task<ProductDTO> AddProductAsync(ProductDTO productDto)
        {
            try
            {
                if (productDto == null)
                    throw new ValidationException("Product data is null.");
                // Validate the product data
                if (string.IsNullOrEmpty(productDto.Name))
                    throw new ValidationException("Product name is required.");
                if (productDto.UnitPrice <= 0)
                    throw new ValidationException("Product unit price must be greater than zero.");
                var product = _mapper.Map<Product>(productDto);
                product = await _repository.AddAsync(product);
                productDto = _mapper.Map<ProductDTO>(product);
                return productDto;
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }            
            catch (Exception ex)
            {
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }
        
        // UpdateProductAsync updates an existing product in the repository.
        public async Task UpdateProductAsync(ProductDTO product) {
            try
            {
                if (product == null)
                    throw new ValidationException("Product data is null.");
                // Validate the product ID
                if (product.Id <= 0)
                    throw new ValidationException("Invalid product ID.");

                // Validate the product data
                if (string.IsNullOrEmpty(product.Name))
                    throw new ValidationException("Product name is required.");
                if (product.UnitPrice <= 0)
                    throw new ValidationException("Product unit price must be greater than zero.");

                var existingProduct = await _repository.GetByIdAsync(product.Id);
                if (existingProduct == null)
                {
                    throw new KeyNotFoundException($"Product with ID {product.Id} not found.");
                }
                // Map the updated properties from the DTO to the existing product entity
                existingProduct.Name = product.Name;
                existingProduct.UnitPrice = product.UnitPrice;
                existingProduct.Description = product.Description;
                existingProduct.Status = product.Status;
                await _repository.UpdateAsync(existingProduct);
            }
            catch (KeyNotFoundException ex)
            {
                throw new KeyNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Internal server error: {ex.Message}");
            }
        }
        
        // DeleteProductAsync deletes a product by its ID from the repository.
        // It does not return any value.
        public Task DeleteProductAsync(int id) => _repository.DeleteAsync(id);
    }
}