using Loushop.Data;
using Loushop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Ajax.Utilities;

namespace Loushop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private LouShopContext _contex;
        


        public object Products { get; private set; }

        public HomeController(ILogger<HomeController> logger, LouShopContext contex)
        {
            _logger = logger;
            _contex = contex;
        }

        public IActionResult Index()
        {
            var Products = _contex.Products 

                .ToList();
            return View(Products);
        }

        public IActionResult Detail(int id)
        {
            var Product = _contex.Products
                .Include(p => p.Item)
                .SingleOrDefault(p => p.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            var categories = _contex.Products
                .Where(p => p.Id == id)
                .SelectMany(c => c.categoryToProducts)
                .Select(ca => ca.Category)
                .ToList();

            var vm = new DetailsViewModel()
            {
                product = Product,
                Categories = categories

            };

            return View(vm);
        }
        [Authorize]
        public IActionResult AddToCart(int itemId)
        {
            var product = _contex.Products.Include(navigationPropertyPath: p => p.Item).SingleOrDefault(p => p.ItemId == itemId);
            if (product != null)
            {
                int userId = int.Parse(User.FindFirstValue(claimType: ClaimTypes.NameIdentifier).ToString());
                var order = _contex.Orders.FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);
             if (order !=null)
                {
                    var OrderDetail =
                        _contex.OrderDetails.FirstOrDefault(d=>
                        d.OrderId==order.OrderId && d.ProductId== product.Id );

                    if( OrderDetail!=null)
                    {
                        OrderDetail.Count += 1;
                    }

                    else

                    {
                        _contex.OrderDetails.Add(new OrderDetail()
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
                    order = new Order()
                    {
                        IsFinaly= false,
                        CreateDate = DateTime.Now,
                        UserId = userId
                    };
                    _contex.Orders.Add(order);
                    _contex.SaveChanges();
                    _contex.OrderDetails.Add(new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        ProductId = product.Id,
                        Price = product.Item.Price,
                        Count=1
                    });
                }
               
            }
            return RedirectToAction("ShowCart");
        }
        [Authorize]
        public IActionResult ShowCart()
        {

            int userId = int.Parse(User.FindFirstValue(claimType: ClaimTypes.NameIdentifier).ToString());
            var order = _contex.Orders.Where(o => o.UserId == userId)
                .Include(navigationPropertyPath: o => o.OrderDetails)
                .ThenInclude(c => c.Product).FirstOrDefault();
            return View(order);
        }

        public IActionResult RemoveCart(int detailId)
        {
            var orderDetail = _contex.OrderDetails.Find(detailId);
            _contex.Remove(orderDetail);
            _contex.SaveChanges();
            return RedirectToAction("ShowCart");
        }


        [Route(template: "Discounts")]
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
    }
}
