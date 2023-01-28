using BusinessLayer.Abstract;
using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using ShopAppUI.Models;
using ShopAppUI.ViewModels;

namespace ShopAppUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult List(string category, int page=1) //page null olmaması için değer verdik
        {
            const int pageSize = 2; //sayfa başına gösterilecek ürün sayısı için 

            var productViewModel = new ProductListViewModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage= page,
                    ItemsPerPage= pageSize,
                    CurrentCategory= category
                },
                Products = _productService.GetProductsByCategory(category,page,pageSize)
            };

            return View(productViewModel);
        }
        public IActionResult Details(string url)
        {
            if (url == null)
            {
                return NotFound();
            }
            Product product = _productService.GetProductDetails(url);

            if (product == null)
            {
                return NotFound();
            }
            return View(new ProductDetailModel
            {
                Product = product,
                Categories = product.ProductCategories.Select(x=>x.Category).ToList()
            });
        }
        public IActionResult Search(string q)
        {
            var productViewModel = new ProductListViewModel()
            {
                Products = _productService.GetSearchResult(q)
            };

            return View(productViewModel);
        }
    }
}
