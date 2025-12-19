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
    public class DeleteModel : PageModel
    {
        private readonly Loushop.Data.LouShopContext _context;

        public DeleteModel(Loushop.Data.LouShopContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.categories.FindAsync(id);

            if (Category != null)
            {
                _context.categories.Remove(Category);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
