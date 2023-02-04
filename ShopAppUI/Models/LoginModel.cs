using System.ComponentModel.DataAnnotations;

namespace ShopAppUI.Models
{
    public class LoginModel
    {
        //public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }


    }
}
