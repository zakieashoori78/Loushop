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
using Loushop.services;
using Loushop.Dtos.RequestDto;
using System.Collections.Generic;
using Loushop.ViewModels;
using Loushop.ViewModels.Search;

namespace Loushop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LouShopContext _context;
        private readonly IPaymentService paymentService;
        public HomeController(ILogger<HomeController> logger, LouShopContext context, IPaymentService paymentService)
        {
            _logger = logger;
            _context = context;
            this.paymentService = paymentService;
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
        public async Task<IActionResult> Payment()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);
            if (order == null)
                return NotFound();

            var dto = new PaymentDto
            {
                Amount = order.OrderDetails.Sum(d => d.Price * d.Count),
                UserId = "",
                OrderId = order.OrderId
            };
            var payment = await paymentService.Request(dto);

            if (!string.IsNullOrEmpty(payment.payLink))
            {
                return Redirect(payment.payLink);
            }
            else
            {
                return BadRequest();
            }
        }

        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"].ToString();
                var order = _context.Orders.Include(o => o.OrderDetails)
                    .FirstOrDefault(o => o.OrderId == id);
            }

            return NotFound();
        }

        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View(new List<SearchDetailsViewModel>()); // اگر کوئری خالی باشد، لیست خالی بازگردانده می‌شود
            }

            var results = (from product in _context.Products
                           join item in _context.Items on product.ItemId equals item.Id
                           join categoryToProduct in _context.CategoryToProducts on product.Id equals categoryToProduct.ProductId
                           join category in _context.categories on categoryToProduct.CategoryId equals category.Id
                           where product.Name.Contains(query) || product.Description.Contains(query) || category.Name.Contains(query)
                           select new SearchDetailsViewModel
                           {
                               Product = new SearchProductViewModel
                               {
                                   Id = product.Id,
                                   Name = product.Name,
                                   Description = product.Description,
                                   Item = new SearchItemViewModel
                                   {
                                       Price = item.Price,
                                       QuantityInStoke = item.QuantityInStoke
                                   }
                               },
                               Categories = new List<SearchCategoryViewModel>
                       {
                           new SearchCategoryViewModel
                           {
                               Name = category.Name,
                               Description = category.Description
                           }
                       }
                           }).ToList();

            return View(results);
        }



    }
}
