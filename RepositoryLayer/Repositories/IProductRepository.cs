using RepositoryLayer.Models;

namespace RepositoryLayer.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();

        Product? GetById(int id);

        Product Add(Product product);
    }
}
