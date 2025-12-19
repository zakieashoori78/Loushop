using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Loushop.Data;
using Loushop.Models;

namespace Loushop.Pages.Admin.Category
{
    public class DetailsModel : PageModel
    {
        private readonly Loushop.Data.LouShopContext _context;

        public DetailsModel(Loushop.Data.LouShopContext context)
        {
            _context = context;
        }

        public Loushop.Models.Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.categories.FirstOrDefaultAsync(m => m.Id == id);

            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
