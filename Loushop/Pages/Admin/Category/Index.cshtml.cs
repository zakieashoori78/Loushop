using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Loushop.Pages.Admin.Category
{
    public class IndexModel : PageModel
    {
        private readonly Loushop.Data.LouShopContext _context;

        public IndexModel(Loushop.Data.LouShopContext context)
        {
            _context = context;
        }

        public IList<Loushop.Models.Category> Category { get;set; }

        public async Task OnGetAsync()
        {
            Category = await _context.categories.ToListAsync();
        }
    }
}
