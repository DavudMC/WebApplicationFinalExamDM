using Microsoft.AspNetCore.Identity;
using WebApplicationFinalExamDM.Enums;
using WebApplicationFinalExamDM.Models;
using WebApplicationFinalExamDM.ViewModels.UserViewModels;

namespace WebApplicationFinalExamDM.Helpers
{
    public class DbContextInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AdminVM _adminVM;
        public DbContextInitializer(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _adminVM = _configuration.GetSection("AdminSettings").Get<AdminVM>() ?? new();
        }
        public async Task InitializeDatabaseAsync()
        {
            await CreateRoles();
            await CreateAdmin();
        }

        private async Task CreateAdmin()
        {
            AppUser user = new()
            {
                UserName = _adminVM.UserName,
                FullName = _adminVM.FullName,
                Email = _adminVM.Email
            };
            var result = await _userManager.CreateAsync(user, _adminVM.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, IdentityRoles.Admin.ToString());
            }
        }

        private async Task CreateRoles()
        {
            foreach (var role in Enum.GetNames(typeof(IdentityRoles)))
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = role
                });
            }
        }
    }
}
