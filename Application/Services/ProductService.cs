using Aviva.PaymentOrders.Domain.Contracts;
using Aviva.PaymentOrders.Domain.Entities;
using Aviva.PaymentOrders.DataInfrastructure.Repositories;
using Aviva.PaymentOrders.Application.Adapters;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public Task<IEnumerable<ProductDTO>> GetAllProductsAsync() {
            var products = _repository.GetAllAsync().Result;
            return Task.FromResult(_mapper.Map<List<ProductDTO>>(products.ToList()).AsEnumerable());
        }
        
        // GetProductByIdAsync retrieves a product by its ID and maps it to a ProductDTO.
        // If the product is not found, it returns null.
        public Task<ProductDTO> GetProductByIdAsync(int id) {
            var product = _repository.GetByIdAsync(id).Result;
            return Task.FromResult(_mapper.Map<ProductDTO>(product));
        }

        // AddProductAsync maps the ProductDTO to a Product entity and adds it to the repository.
        public Task AddProductAsync(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            return _repository.AddAsync(product);
        }
        
        // UpdateProductAsync updates an existing product in the repository.
        public Task UpdateProductAsync(ProductDTO product) {
            var existingProduct = _repository.GetByIdAsync(product.Id).Result;
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product with ID {product.Id} not found.");
            }
            // Map the updated properties from the DTO to the existing product entity
            existingProduct.Name = product.Name;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.Description = product.Description;
            existingProduct.Status = product.Status;
            return _repository.UpdateAsync(existingProduct);
        }
        
        // DeleteProductAsync deletes a product by its ID from the repository.
        // It does not return any value.
        public Task DeleteProductAsync(int id) => _repository.DeleteAsync(id);
    }
}