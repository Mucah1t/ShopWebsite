using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IProductService : IValidator<Product>
    {
        Product GetById(int id);
        Product GetByIdWithCategories(int id);
        Product GetProductDetails(string url);
        List<Product> GetHomePageProducts();
        List<Product> GetAll();
        List<Product> GetProductsByCategory(string name, int page, int pageSize);
        List<Product> GetSearchResult(string searchString);

        void Create(Product entity);

        void Update(Product entity);
        void Delete(Product entity);
        int GetCountByCategory(string category);
        bool Update(Product entity, int[] categoryIds);
    }
}
