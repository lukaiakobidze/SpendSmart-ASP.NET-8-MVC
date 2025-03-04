using Microsoft.AspNetCore.Identity;

namespace SpendSmart.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; } 
    }
}
