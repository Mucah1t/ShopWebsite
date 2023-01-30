using BusinessLayer.Abstract;
using EntityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopAppUI.Models;
using ShopAppUI.ViewModels;

namespace ShopAppUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategorryService _categoryService;

        public AdminController(IProductService productService, ICategorryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }


        public IActionResult ProductList() //CRUD List Operation
        {
            return View(new ProductListViewModel()
            {
                Products = _productService.GetAll()
            });
        }
        public IActionResult CategoryList() //CRUD List Operation
        {
            return View(new CategoryListViewModel()
            {
                Categories = _categoryService.GetAll()
            });
        }
        [HttpGet] //When page loads
        public IActionResult ProductCreate()
        {
            return View();
        }
        [HttpPost] // When the action has taken(clicking button)
        public IActionResult ProductCreate(ProductModel model)
        {
            //if (ModelState.IsValid)
            //{
            var entity = new Product() //related entities are filled with the necessary data
            {
                Name = model.Name,
                Url = model.Url,
                Price = model.Price,
                Description = model.Description,
                ImageUrl = model.ImageUrl

            };
            _productService.Create(entity);

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli ürün eklendi.",
                AlertType = "success"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("ProductList");
            //  }
            //return View(model);
        }

        [HttpGet] //When page loads
        public IActionResult CategoryCreate()
        {
            return View();
        }
        [HttpPost] // When the action has taken(clicking button)
        public IActionResult CategoryCreate(CategoryModel model)
        {
            var entity = new Category() //related entities are filled with the necessary data
            {
                Name = model.Name,
                Url = model.Url,
            };
            _categoryService.Create(entity);

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli kategori eklendi.",
                AlertType = "success"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public IActionResult ProductEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var entity = _productService.GetByIdWithCategories((int)id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Price = entity.Price,
                ImageUrl = entity.ImageUrl,
                Description = entity.Description,
                IsApproved = entity.IsApproved,
                IsHome= entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoryIds,IFormFile file)
        {

            //if (ModelState.IsValid)
            //{
            var entity = _productService.GetById(model.ProductId);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Name = model.Name;
            entity.Url = model.Url;
            entity.Price = model.Price;
            entity.Description = model.Description;
            entity.IsHome = model.IsHome;
            entity.IsApproved = model.IsApproved;

            if (file!=null)
            {
                var extension=Path.GetExtension(file.FileName);
                var randomName = string.Format($"{Guid.NewGuid()}{extension}");
                entity.ImageUrl = randomName;//we assigned the relevant filename to the entity
                var path=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img",randomName); //we need to give a way to save the picture

                using (var stream = new FileStream(path, FileMode.Create))
                {
                   await file.CopyToAsync(stream);
                } 
            }

            if (_productService.Update(entity, categoryIds))
            {
                CreateMessage("kayıt güncellendi", "success");
                return RedirectToAction("ProductList");
            }
            CreateMessage(_productService.ErrorMessage, "danger");
            //}
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            var entity = _categoryService.GetByIdWithProducts((int)id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.CategoryId);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Name = model.Name;
            entity.Url = model.Url;




            _categoryService.Update(entity);

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli kategori güncellendi.",
                AlertType = "success"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("CategoryList");

        }
        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }
            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli ürün silindi.",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("ProductList");

        }
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }
            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} isimli kategori silindi.",
                AlertType = "danger"
            };

            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("CategoryList");

        }
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/admin/categories/" + categoryId);
        }
        private void CreateMessage(string message, string alerttype)
        {
            var msg = new AlertMessage()
            {
                Message = message,
                AlertType = alerttype
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
        }
    }
}
