using Loushop.Data;
using Loushop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ZarinpalSandbox;

namespace Loushop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LouShopContext _context;

        public HomeController(ILogger<HomeController> logger, LouShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Detail(int id)
        {
            var product = _context.Products
                .Include(p => p.Item)
                .SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var categories = _context.Products
                .Where(p => p.Id == id)
                .SelectMany(c => c.categoryToProducts)
                .Select(ca => ca.Category)
                .ToList();

            var vm = new DetailsViewModel()
            {
                Product = product,
                Categories = categories
            };

            return View(vm);
        }

        [Authorize]
        public IActionResult AddToCart(int itemId)
        {
            var product = _context.Products
                .Include(p => p.Item)
                .SingleOrDefault(p => p.ItemId == itemId);

            if (product != null)
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var order = _context.Orders.FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);

                if (order != null)
                {
                    var orderDetail = _context.OrderDetails
                        .FirstOrDefault(d => d.OrderId == order.OrderId && d.ProductId == product.Id);

                    if (orderDetail != null)
                    {
                        orderDetail.Count += 1;
                    }
                    else
                    {
                        _context.OrderDetails.Add(new OrderDetail
                        {
                            OrderId = order.OrderId,
                            ProductId = product.Id,
                            Price = product.Item.Price,
                            Count = 1
                        });
                    }
                }
                else
                {
                    order = new Order
                    {
                        IsFinaly = false,
                        CreateDate = DateTime.Now,
                        UserId = userId
                    };

                    _context.Orders.Add(order);
                    _context.SaveChanges();

                    _context.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = product.Id,
                        Price = product.Item.Price,
                        Count = 1
                    });
                }

                _context.SaveChanges();
            }

            return RedirectToAction("ShowCart");
        }

        [Authorize]
        public IActionResult ShowCart()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = _context.Orders
                .Where(o => o.UserId == userId && !o.IsFinaly)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefault();

            return View(order);
        }

        public IActionResult RemoveCart(int detailId)
        {
            var orderDetail = _context.OrderDetails.Find(detailId);

            if (orderDetail != null)
            {
                _context.OrderDetails.Remove(orderDetail);
                _context.SaveChanges();
            }

            return RedirectToAction("ShowCart");
        }

        [Route("Discounts")]
        public IActionResult Discounts()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Payment()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);

            if (order == null)
                return NotFound();

            var payment = new Payment((int)order.OrderDetails.Sum(d => d.Price));
            var res = payment.PaymentRequest($"پرداخت فاکتور شماره {order.OrderId}", $"http://localhost:44369/Home/OnlinePayment/{order.OrderId}", "Zakie78.ashoori@gmail.com", "09197070750");

            if (res.Result.Status == 100)
            {
                return Redirect($"https://sandbox.zarinpal.com/pg/StartPay/{res.Result.Authority}");
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult OnlinePayment(int id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Request.Query["Status"]) &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                !string.IsNullOrEmpty(HttpContext.Request.Query["Authority"]))
            {
                string authority = HttpContext.Request.Query["Authority"];
                var order = _context.Orders
                    .Include(o => o.OrderDetails)
                    .FirstOrDefault(o => o.OrderId == id);

                var payment = new Payment((int)order.OrderDetails.Sum(d => d.Price));
                var res = payment.Verification(authority).Result;

                if (res.Status == 100)
                {
                    order.IsFinaly = true;
                    _context.Orders.Update(order);
                    _context.SaveChanges();

                    ViewBag.Code = res.RefId;
                    return View();
                }
            }

            return NotFound();
        }
    }
}
