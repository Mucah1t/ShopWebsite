using Microsoft.AspNetCore.Identity;

namespace ShopAppUI.Identity
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
