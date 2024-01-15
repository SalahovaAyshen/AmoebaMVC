using Amoeba.Areas.Manage.ViewModels;
using Amoeba.Models;
using Amoeba.Utilities.Enums;
using Amoeba.Utilities.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Amoeba.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _user;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> user, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _user = user;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            if (!registerVM.Name.Check())
            {
                ModelState.AddModelError("Name", "Name can't contain number");
                return View();
            }
            if (!registerVM.Surname.Check())
            {
                ModelState.AddModelError("Surname", "Surname can't contain number");
                return View();
            }
            string email = registerVM.Email;
            Regex regex = new Regex(@"^(([0-9a-z]|[a-z0-9(\.)?a-z]|[a-z0-9])){1,}(\@)[a-z((\-)?)]{1,}(\.)([a-z]{1,}(\.))?([a-z]{2,3})$");
            if (!regex.IsMatch(email))
            {
                ModelState.AddModelError("Email", "Wrong email");
                return View();
            }
            AppUser user = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
                Email = email,
            };
            IdentityResult result = await _user.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, item.Description);
                    return View();
                }
            }

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM, string returnurl)
        {
            AppUser user = await _user.FindByNameAsync(loginVM.UsernameOrEmail);
            if (user == null)
            {
                user = await _user.FindByEmailAsync(loginVM.UsernameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError(String.Empty, "Username, email or password is incorrect");
                    return View();
                }
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(String.Empty, "Locked");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "wrong");
                return View();
            }
            if (returnurl == null)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return Redirect(returnurl);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Dashboard");
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach (UserRole item in Enum.GetValues(typeof(UserRole)))
            {
                if(!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = item.ToString()
                    });
                }
            }
            return RedirectToAction("Index", "Dashboard");
        }
    }
}
