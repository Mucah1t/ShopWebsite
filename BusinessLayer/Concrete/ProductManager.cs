using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.EfCore;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void Create(Product entity)
        {
            _productRepository.Create(entity);
        }

        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        public List<Product> GetAll()
        {
           return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
           return _productRepository.GetById(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
           return _productRepository.GetHomePageProducts();
        }

        public Product GetProductDetails(string url)
        {
            return _productRepository.GetProductDetails(url);
        }

        public List<Product> GetProductsByCategory(string name, int page ,int pageSize)
        {
            return _productRepository.GetProductsByCategory(name,page ,pageSize);
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _productRepository.GetSearchResult(searchString);    
        }

        public void Update(Product entity)
        {
           _productRepository.Update(entity);
        }

        public bool Update(Product entity, int[] categoryIds)
        {
            if (Validation(entity))
            {
                if (categoryIds.Length==0)
                {
                    ErrorMessage += "Ürün için en az bir kategori seçmelisiniz.";
                    return false;
                }
                _productRepository.Update(entity, categoryIds);

                return true;
            }
            return false;
        }

        public string ErrorMessage { get; set; }
        public bool Validation(Product entity)
        {
            var isvalid = true;

            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "Ürün İsmi Girmelisiniz.\n";
                isvalid = false;
            }
            if (entity.Price < 0)
            {
                ErrorMessage += "Ürün Fiyatı Negatif Olamaz.\n";
                isvalid = false;
            }

            return isvalid;
        }
    }
}
