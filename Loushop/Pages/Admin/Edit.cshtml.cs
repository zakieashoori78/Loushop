using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Loushop.Data;
using Loushop.Models;
using Loushop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Loushop.Pages.Admin
{
    public class EditModel : PageModel
       
    {
        private LouShopContext _context;
        public EditModel(LouShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AddEditViewModel Product { get; set; }

        [BindProperty]
        public List<int> selectedGroups { get; set; }
        public List<int> GoupsProduct { get; set; }
        public void OnGet(int id)
        {
            var product = _context.Products.Include(p => p.Item)
                .Where(p => p.Id == id)
                .Select(s => new AddEditViewModel()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    QuantityInStock = s.Item.QuantityInStoke,
                    Price = s.Item.Price
                }).FirstOrDefault();

            Product = product;
            product.Categories = _context.categories.ToList();
            GoupsProduct = _context.CategoryToProducts.Where(c => c.ProductId == id)
                .Select(s => s.CategoryId).ToList();
            
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Product.Categories = _context.categories.ToList();
                return Page();
            }

            if (Product.Categories == null || Product.Categories.Count == 0)
            {
                Product.Categories = _context.categories.ToList();
                ModelState.AddModelError("Product.Categories", "لطفاً حداقل یک دسته‌بندی انتخاب کنید.");
            }
           
            var product = _context.Products.Find(Product.Id);
            if (product == null || product.ItemId == 0)
            {
                Product.Categories = _context.categories.ToList();
                return Page();
            }

            var item = _context.Items.First(p => p.Id == product.ItemId);
            if (item == null)
            {
                Product.Categories = _context.categories.ToList();
                return Page();
            }

            product.Name = Product.Name;
            product.Description = Product.Description;
            item.Price = Product.Price;
            item.QuantityInStoke = Product.QuantityInStock;
            _context.SaveChanges();

            if (Product.Picture?.Length > 0)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    product.Id + Path.GetExtension(Product.Picture.FileName));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Product.Picture.CopyTo(stream);
                }
            }

            _context.CategoryToProducts.Where(c => c.ProductId == Product.Id).ToList()
                .ForEach(g => _context.CategoryToProducts.Remove(g));

            if (selectedGroups.Any() && selectedGroups.Count > 0)
            {
                foreach (int gr in selectedGroups)
                {
                    _context.CategoryToProducts.Add(new CategoryToProduct()
                    {
                        CategoryId = gr,
                        ProductId = Product.Id
                    });
                }

                _context.SaveChanges();
            }
            return RedirectToPage("Index");
        }
    }
}
