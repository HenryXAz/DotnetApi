using Microsoft.AspNetCore.Identity;

namespace DotnetApi.Models
{
    public class AppUser : IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = []; 
    }
}