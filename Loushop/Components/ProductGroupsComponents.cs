using Loushop.Data;
using Loushop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loushop.Components
{
    public class ProductGroupsComponents: ViewComponent
    {
        private LouShopContext _context;


        public ProductGroupsComponents(LouShopContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = _context.categories
                .Select(c => new ShowGroupViewModel()
                {
                    GroupId = c.Id,
                    Name = c.Name,
                    ProductCount = _context.CategoryToProducts.Count(g => g.CategoryId == c.Id)
                }).ToList();
            return View("/Views/Components/ProductGroupsComponent.cshtml", categories);
        }
    }
}
