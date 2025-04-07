using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Loushop.Data;
using Loushop.Models;

namespace Loushop.Pages.Admin.ManageUser
{
    public class IndexModel : PageModel
    {
        private readonly Loushop.Data.LouShopContext _context;

        public IndexModel(Loushop.Data.LouShopContext context)
        {
            _context = context;
        }

        public IList<Users> Users { get;set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
        }
    }
}
