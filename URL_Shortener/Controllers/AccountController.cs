
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using URL_Shortener.Data;
using URL_Shortener.Models;
using URL_Shortener.Services.AccountServices;
using URL_Shortener.Services.UsersServices;
using URL_Shortener.ViewModels;

namespace URL_Shortener.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersCntrollerService _usersCntrollerService;
        private readonly IAccountControllerService _accountControllerService;
        public AccountController(IUsersCntrollerService usersCntrollerService, 
            IAccountControllerService accountControllerService)
        {
            _usersCntrollerService = usersCntrollerService;
            _accountControllerService = accountControllerService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = await _accountControllerService.GetUserByEmailAndPasswordAsync(model.Email, model.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Incorrect login and(or) password");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            User? user = await _usersCntrollerService.GetUserByEmailAsync(model.Email);
            if (user == null)
            {
                user = new User { FullName = model.FullName, 
                    Email = model.Email, 
                    Password = model.Password
                };
                Role? userRole = await _accountControllerService.GetUserRoleWithUserValueAsync();

                if (userRole != null) 
                    user.Role = userRole;

                await _accountControllerService.AddNewUserAsync(user);

                await Authenticate(user);

                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "Incorrect login and(or) password");
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}
