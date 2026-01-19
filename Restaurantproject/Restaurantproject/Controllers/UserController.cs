using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Restaurantproject.Models;
using System.Threading.Tasks;


namespace Restaurantproject.Controllers
{
    public class UserController : Controller
    {
        private UserManager<IdentityUser> _UserManager;
        private SignInManager<IdentityUser> _SignInManager;
        private RoleManager<IdentityRole> _RoleManager;
        public UserController(UserManager<IdentityUser> UserManager, SignInManager<IdentityUser> SignInManager, RoleManager<IdentityRole> RoleManager)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
            _RoleManager = RoleManager;

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserAccount model)
        {
            if (ModelState.IsValid)
            {
                // تاكد إن كان المستخدم موجوداً بالفعل
                var existingUser = await _UserManager.FindByEmailAsync(model.User_Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "This email address is already registered.");
                    return View(model);
                }
                var user = new IdentityUser
                {
                    Email = model.User_Email,
                    UserName = model.User_Email,

                };

                var result = await _UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _SignInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }


            }
            return View(model);
        }

        [HttpGet]
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> login(loginAcc model)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserManager.FindByEmailAsync(model.Email);

                if (user != null)
                {          
                    var result = await _SignInManager.PasswordSignInAsync(
                        user.UserName, model.password, true, true);
                 
                    if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }


                };    

               

            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> logout()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        [AllowAnonymous]

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
