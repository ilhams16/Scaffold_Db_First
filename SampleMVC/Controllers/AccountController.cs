using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MyRESTServices.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using SampleMVC.Services;
using System.Security.Claims;

namespace SampleMVC.Controllers
{

    public class AccountController : Controller
    {
        private readonly IUserBLL _userBLL;
        private readonly IRoleBLL _roleBLL;
        private readonly IAccountServices _accountServices;

        public AccountController(IUserBLL userBLL, IRoleBLL roleBLL, IAccountServices accountServices)
        {
            _userBLL = userBLL;
            _roleBLL = roleBLL;
            _accountServices = accountServices;
        }

        public IActionResult Login(string ReturnUrl = "")
        {
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                ViewBag.ReturnUrl = ReturnUrl;
            }
            return View();
        }

        // [HttpPost]
        // public async Task<IActionResult> Login(LoginDTO loginDTO)
        // {
        //     var user = _userBLL.LoginMVC(loginDTO);
        //     if (user != null)
        //     {
        //         var userwithroles = _userBLL.GetUserWithRoles(user.Username);

        //         var claims = new List<Claim>
        //         {
        //             new Claim(ClaimTypes.NameIdentifier,user.Username),
        //             new Claim(ClaimTypes.Name,$"{user.FirstName} {user.LastName}")
        //         };

        //         //tambahkan semua role claim
        //         if (userwithroles != null)
        //         {
        //             foreach (var role in userwithroles.Roles)
        //             {
        //                 claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
        //             }
        //         }

        //         var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //         var principal = new ClaimsPrincipal(identity);
        //         await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        //             principal, new AuthenticationProperties { IsPersistent = loginDTO.RememberLogin });

        //         if (string.IsNullOrEmpty(loginDTO.ReturnUrl))
        //             return RedirectToAction("Index", "Home");
        //         else
        //             return LocalRedirect(loginDTO.ReturnUrl);

        //         //return RedirectToAction("Index", "Home");
        //     }
        //     else
        //     {
        //         ViewBag.Error = "Invalid username or password";
        //         return View();
        //     }
        // }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _accountServices.Login(loginDTO);
            if (user != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

    }
}
