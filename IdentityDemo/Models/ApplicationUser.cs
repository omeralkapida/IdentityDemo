using Microsoft.AspNetCore.Identity;

namespace IdentityDemo.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? FullName { get; set; }
        public int? ParentUserId { get; set; }
        public ApplicationUser? ParentUser { get; set; }

        public ICollection<ApplicationUser>? SubUsers { get; set; }
    }
}
