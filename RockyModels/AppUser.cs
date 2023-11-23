using Microsoft.AspNetCore.Identity;
using System.Data.Common;

namespace RockyModels
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
