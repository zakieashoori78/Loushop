using Loushop.Data;
using Loushop.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Loushop.Controllers
{
    public class ProfileController : Controller
    {
        // اکشن برای نمایش پروفایل کاربر
        private readonly LouShopContext _context;

        public ProfileController(LouShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // یا از روش دیگری برای گرفتن UserId استفاده کنید.

            // پیدا کردن سفارشات غیر نهایی برای کاربر
            var orders = await _context.Orders
                .Where(o => o.UserId == userId && !o.IsFinaly)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .ToListAsync();

            // محاسبه تعداد سفارشات و مجموع خرید
            var totalOrders = orders?.Sum(o => o.OrderDetails?.Count) ?? 0;  // تعداد کل سفارشات غیر نهایی
            var totalSpent = orders?.Sum(o => o.OrderDetails?.Sum(od => od.Price * od.Count)) ?? 0;  // مجموع خرید

            // پیدا کردن اطلاعات کاربر
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            // مدل داده‌ای برای ارسال به ویو
            var model = new UserProfileViewModel
            {
                Email = user.Email,
                RegisterDate = user.RegisterDate,
                TotalOrders = totalOrders,
                TotalSpent = totalSpent,
                Orders = orders.Select(o => new OrderReportViewModel
                {
                    OrderId = o.OrderId,
                    CreateDate = o.CreateDate,
                    IsFinaly = o.IsFinaly,
                    TotalAmount = o.OrderDetails.Sum(od => od.Price * od.Count),
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailViewModel
                    {
                        ProductName = od.Product.Name,
                        Price = od.Price,
                        Count = od.Count
                    }).ToList()
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)); // گرفتن userId از Claims
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    return NotFound("کاربر یافت نشد.");
                }

                // بررسی رمز عبور فعلی
                if (user.Password != model.CurrentPassword)  // فرض کنیم که رمز عبور به صورت متنی ذخیره شده است
                {
                    ModelState.AddModelError("", "رمز عبور فعلی اشتباه است.");
                    return View(model);
                }

                // تغییر رمز عبور
                user.Password = model.NewPassword;
                _context.Update(user);
                await _context.SaveChangesAsync();

                // در صورتی که رمز عبور با موفقیت تغییر یافت، کاربر باید دوباره وارد شود
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Redirect(url: "/Account/Login");
            }

            return View(model);
        }
    }
}
