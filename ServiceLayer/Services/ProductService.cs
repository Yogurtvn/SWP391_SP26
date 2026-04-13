using RepositoryLayer.Models;
using RepositoryLayer.Repositories;

namespace ServiceLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Product> GetAll() => _repository.GetAll();

        public Product? GetById(int id) => _repository.GetById(id);

        public Product Create(string name, decimal price)
        {
            var product = new Product
            {
                Name = name,
                Price = price
            };

            return _repository.Add(product);
        }
    }
}
