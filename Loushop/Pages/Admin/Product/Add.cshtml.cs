using System.Collections.Generic;
using System.IO;
using System.Linq;
using Loushop.Data;
using Loushop.Models;
using Loushop.ViewModels;
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
                QuantityInStocke = Product.QuantityInStock
            };
            _context.Add(item);
            _context.SaveChanges();
            var imageFileName = item.Id + Path.GetExtension(Product.Picture.FileName);

            var pro = new Product()
            {
                Name = Product.Name,
                Item = item,
                Description = Product.Description,
                ImagePath = "/images/" + imageFileName
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
                var savePath = Path.Combine("wwwroot/images", imageFileName);
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    Product.Picture.CopyTo(stream);
                }
            }
           
            return RedirectToPage("index");
        }
    }
}
