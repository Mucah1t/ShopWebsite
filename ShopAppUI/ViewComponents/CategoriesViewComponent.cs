using System.Collections.Generic;
using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;


namespace shopapp.webui.ViewComponents
{
    public class CategoriesViewComponent:ViewComponent
    {
        private ICategorryService _categpryService;

        public CategoriesViewComponent(ICategorryService categpryService)
        {
            _categpryService = categpryService;
        }

        public IViewComponentResult Invoke()
        {
            if (RouteData.Values["category"] != null)
                ViewBag.SelectedCategory = RouteData?.Values["category"];

            return View(_categpryService.GetAll());
         
        }
    }
}