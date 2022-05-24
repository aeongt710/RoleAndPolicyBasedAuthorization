using System.ComponentModel.DataAnnotations;

namespace IdentityUserManagement.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Pages { get; set; }
        public string Description { get; set; }
    }
}
