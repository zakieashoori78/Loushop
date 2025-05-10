using Loushop.Data;
using Loushop.Models;
using Loushop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Loushop.Controllers
{
    //[Role("Admin")]
    public class AdminController : Controller
    {
        private readonly LouShopContext _context;

        public AdminController(LouShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // گزارش محصولات
            var productReports = _context.Products
                .Include(p => p.Item)
                .Select(p => new ProductReportViewModel
                {
                    ProductName = p.Name,
                    Price = p.Item.Price,
                    Stock = p.Item.QuantityInStocke,
                    OrderCount = p.OrderDetails.Count
                }).ToList();

            // گزارش کاربران
            var userReports = _context.Users
                .Select(u => new UserReportViewModel
                {
                    Email = u.Email,
                    RegisterDate = u.RegisterDate,
                    IsAdmin = u.IsAdmin,
                    OrderCount = u.Orders.Count
                }).ToList();

            // گزارش‌های خلاصه
            var totalProducts = _context.Products.Count();
            var productWithHighestStock = _context.Products.OrderByDescending(p => p.Item.QuantityInStocke).FirstOrDefault()?.Name;
            var productWithLowestStock = _context.Products.OrderBy(p => p.Item.QuantityInStocke).FirstOrDefault()?.Name;
            var bestSellingProduct = _context.Products
                .OrderByDescending(p => p.OrderDetails.Sum(od => od.Count))
                .FirstOrDefault()?.Name;

            var totalUsers = _context.Users.Count();
            var totalAdmins = _context.Users.Count(u => u.IsAdmin);

            var model = new AdminDashboardViewModel
            {
                ProductReports = productReports,
                UserReports = userReports,
                TotalProducts = totalProducts,
                ProductWithHighestStock = productWithHighestStock,
                ProductWithLowestStock = productWithLowestStock,
                BestSellingProduct = bestSellingProduct,
                TotalUsers = totalUsers,
                TotalAdmins = totalAdmins
            };

            return View(model);
        }

    }
}
