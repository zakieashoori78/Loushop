using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Loushop.Data;
using Loushop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Loushop.Pages.Admin
{
    public class AddModel : PageModel
    {

        private LouShopContext _context;
        public AddModel(LouShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AddEditViewModel Product { get; set; }
        [BindProperty]
        public List<int> selectedGroups { get; set; }
        public void OnGet()
        {

            Product = new AddEditViewModel()
            {
                Categories = _context.categories.ToList()
            };
        }

        public IActionResult OnPost()
        {
            if (Product.Categories == null || Product.Categories.Count == 0)
            {
                Product.Categories = _context.categories.ToList();
                ModelState.AddModelError("Product.Categories", "لطفاً حداقل یک دسته‌بندی انتخاب کنید.");
            }
            if (!ModelState.IsValid)
            {
                Product.Categories = _context.categories.ToList();
                return Page();
            }

            var item = new Item()
            {
                Price = Product.Price,
                QuantityInStoke = Product.QuantityInStock
            };
            _context.Add(item);
            _context.SaveChanges();

            var pro = new Product()
            {
                Name = Product.Name,
                Item = item,
                Description = Product.Description,

            };
            _context.Add(pro);
            _context.SaveChanges();
            _context.CategoryToProducts.Add(new CategoryToProduct()
            {
                CategoryId = Product.Categories[0].Id,
                ProductId = pro.Id
            });

            _context.SaveChanges();
            if (Product.Picture?.Length > 0)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    pro.Id + Path.GetExtension(Product.Picture.FileName));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Product.Picture.CopyTo(stream);
                }
            }
           
            return RedirectToPage("index");
        }


    }
}
