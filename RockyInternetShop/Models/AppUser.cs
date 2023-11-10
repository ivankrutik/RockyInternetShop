using Microsoft.AspNetCore.Identity;
using System.Data.Common;

namespace RockyInternetShop.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
