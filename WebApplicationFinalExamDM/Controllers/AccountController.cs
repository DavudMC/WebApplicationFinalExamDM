using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplicationFinalExamDM.Models;
using WebApplicationFinalExamDM.ViewModels.UserViewModels;

namespace WebApplicationFinalExamDM.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager,SignInManager<AppUser> _signInManager) : Controller
    {
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = await _userManager.FindByNameAsync(vm.UserName);
            if(user != null)
            {
                ModelState.AddModelError("UserName", "This username already exist!");
                return View(vm);
            }
            user = await _userManager.FindByEmailAsync(vm.Email);
            if(user != null)
            {
                ModelState.AddModelError(nameof(vm.Email), "This email already exist!");
            }
            AppUser appUser = new()
            {
                UserName = vm.UserName,
                FullName = vm.FullName,
                Email = vm.Email
            };
            var result = await _userManager.CreateAsync(appUser, vm.Password);
            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(vm);
            }
            await _userManager.AddToRoleAsync(appUser, "Member");
            await _signInManager.SignInAsync(appUser, false);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if(user == null)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(vm);
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false);
            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is incorrect");
                return View(vm);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
