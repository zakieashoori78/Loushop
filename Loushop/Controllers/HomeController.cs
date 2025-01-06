using Loushop.Data;
using Loushop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace Loushop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private LouShopContext _contex;
        private static Cart _cart = new Cart();


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
                .Include(navigationPropertyPath: p => p.Item)
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

        public IActionResult AddToCart(int itemId)
        {
            var product = _contex.Products.Include(navigationPropertyPath: p => p.Item).SingleOrDefault(p => p.ItemId == itemId);
            if (product != null)
            {
                var cartItem = new CartItem()
                {
                    Item = product.Item,
                    Quantity = 1
                };

                _cart.addItem(cartItem);
            }
            return RedirectToAction("ShowCart");
        }
        public IActionResult ShowCart()
        {
            var CartVM = new CartViewModel()
            {
                CartItems = _cart.CartItems,
                OrderTotal = _cart.CartItems.Sum(c=>c.getTotalPrice())
            };
            return View(CartVM);
        }

        public IActionResult RemoveCart(int itemId)
        {
            _cart.removeItem(itemId); 
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
