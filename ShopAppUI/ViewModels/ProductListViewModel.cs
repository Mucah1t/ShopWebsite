using EntityLayer;
using System.Collections.Generic;


namespace ShopAppUI.ViewModels
{
    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string CurrentCategory { get; set; }

       public int TotalPages()
        {
            return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); //küsuratlý bölüm sonuçlarý için
        }
    }

    public class ProductListViewModel
    {
        public PageInfo PageInfo { get; set; }
        public List<Product> Products { get; set; }
    }
}