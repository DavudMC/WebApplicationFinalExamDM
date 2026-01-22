using Microsoft.AspNetCore.Identity;

namespace WebApplicationFinalExamDM.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
