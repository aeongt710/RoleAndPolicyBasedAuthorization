using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string? Gender { get; set; }
    }
}
