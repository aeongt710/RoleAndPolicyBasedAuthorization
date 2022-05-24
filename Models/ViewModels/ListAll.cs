using Microsoft.AspNetCore.Identity;

namespace IdentityUserManagement.Models.ViewModels
{
    public class ListAll
    {
        public IEnumerable<ApplicationUser> UserList { get; set; }
        public IEnumerable<IdentityRole> UserRoles { get; set; }
    }
}
