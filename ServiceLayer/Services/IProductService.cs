using RepositoryLayer.Models;

namespace ServiceLayer.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();

        Product? GetById(int id);

        Product Create(string name, decimal price);
    }
}
