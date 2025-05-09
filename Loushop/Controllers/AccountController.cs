using Loushop.Data.Repositories;
using Loushop.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Loushop.ViewModels.RegisterViewModel;
using Microsoft.AspNetCore.Http;
using Loushop.ViewModels;
using Loushop.services;

namespace Loushop.Controllers
{

    public class AccountController : Controller
    {
        private IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;

        public AccountController(IUserRepository userRepository, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
        }

        #region Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {

            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (_userRepository.IsExistUserByEmail(register.Email.ToLower()))
            {
                ModelState.AddModelError(key: "Email", errorMessage: "ایمیل قبلا وارد شده است");
                return View(register);
            }
            Users user = new Users()
            {
                Email = register.Email.ToLower(),
                Password = register.Password,
                IsAdmin = false

            };
            _userRepository.AddUser(user);
            return View("SuccessRegister", register);
        }
        #endregion Register

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userRepository.GetUserForLogin(login.Email.ToLower(), login.Password);
            if (user == null)
            {
                ModelState.AddModelError("Email", "اطلاعات صحیح نیست");
                return View(login);
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.Email),
         new Claim("IsAdmin", user.IsAdmin.ToString()),
    };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = login.RememberMe
            };


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user != null)
                {
                    var body = $@"
                <div style='font-family:Tahoma;'>
                    <p>سلام {user.Email} عزیز،</p>
                    <p>رمز عبور شما: <strong>{user.Password}</strong></p>
                    <p>لطفاً پس از ورود، رمز خود را تغییر دهید.</p>
                    <br/>
                    <p style='color:#888;'>با احترام<br/>تیم پشتیبانی Loushop</p>
                </div>";
                    var subject = "بازیابی رمز عبور - فروشگاه Loushop";

                    await _emailSender.SendEmailAsync(email, subject, body);

                    TempData["EmailSuccess"] = "رمز عبور با موفقیت به ایمیل شما ارسال شد. لطفاً ایمیل خود را بررسی کنید.";
                }
                else
                {
                    TempData["EmailError"] = "کاربری با این ایمیل یافت نشد.";
                }
            }
            catch
            {
                TempData["EmailError"] = "در ارسال ایمیل مشکلی رخ داد. لطفاً دوباره تلاش کنید.";
            }

            return RedirectToAction("Login");
        }


        #endregion
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(url: "/Account/Login");
        }

    }
}










