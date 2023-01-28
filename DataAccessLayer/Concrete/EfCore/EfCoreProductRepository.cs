using DataAccessLayer.Abstract;
using EntityLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccessLayer.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public Product GetByIdWithCategories(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                                .Where(i => i.ProductId == id)
                                .Include(i => i.ProductCategories)
                                .ThenInclude(i => i.Category)
                                .FirstOrDefault();
            }
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.Where(i=>i.IsApproved).AsQueryable();

                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                                    .Include(i => i.ProductCategories)
                                    .ThenInclude(i => i.Category)
                                    .Where(i => i.ProductCategories.Any(a => a.Category.Url == category));
                }

                return products.Count();
            }
        }

        public List<Product> GetHomePageProducts()
        {
            using (var context = new ShopContext())
            {
                return context.Products.Where(x=>x.IsApproved&& x.IsHome==true).ToList();
            }
        }

        public List<Product> GetPopularProducts()
        {
           using(var context = new ShopContext())
            {
                return context.Products.ToList();
            }
        }

        public Product GetProductDetails(string url)
        {
            using (var context = new ShopContext())
            {
                return context.Products.Where(i => i.Url == url)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            using (var context = new ShopContext())
            {
                var products = context
                    .Products
                    .Where(x=>x.IsApproved==true)
                    .AsQueryable();
                if (!string.IsNullOrEmpty(name))
                {
                    products = products
                                      .Include(i => i.ProductCategories)
                                      .ThenInclude(i => i.Category)
                                      .Where(i => i.ProductCategories
                                      .Any(a => a.Category.Url == 
                                      name));
                                      
                }
                return products.Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
        }

        public List<Product> GetSearchResult(string searchString)
        {
            using (var context = new ShopContext())
            {
                var products = context
                    .Products
                    .Where(i => i.IsApproved == true && (i.Name.ToLower().Contains(searchString.ToLower()) || i.Description.ToLower().Contains(searchString.ToLower())))
                    .AsQueryable();
              
                return products.ToList();
            }
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products
               .Include(i => i.ProductCategories)
                .FirstOrDefault(i => i.ProductId == entity.ProductId);
                if (categoryIds != null)
                {
                    products.Name=entity.Name;
                    products.Description=entity.Description;
                    products.Price=entity.Price;
                    products.Url=entity.Url;
                    products.ImageUrl=entity.ImageUrl;
                    products.IsApproved=entity.IsApproved;
                    products.IsHome=entity.IsHome;

                    products.ProductCategories = categoryIds.Select(catid=>new ProductCategory()
                    {
                        ProductId=entity.ProductId,
                        CategoryId =catid
                    }).ToList();

                    context.SaveChanges();
                }
            }
        }
    }
}
