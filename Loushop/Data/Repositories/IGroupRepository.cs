using Loushop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loushop.Data.Repositories
{
    public interface IGroupRepository
    {

        IEnumerable<Category> GetAllCategories();
        IEnumerable<ShowGroupViewModel> GetGroupForShow();

    }


    public class GroupRepository : IGroupRepository
    {

        private LouShopContext _context;
        public GroupRepository(LouShopContext context)
        {
            _context = context;
        }
        public IEnumerable<Category> GetAllCategories()
        {
            return _context.categories;
        }

        public IEnumerable<ShowGroupViewModel> GetGroupForShow()
        {
            return _context.categories
                      .Select(c => new ShowGroupViewModel()
                      {
                          GroupId = c.Id,
                          Name = c.Name,
                          ProductCount = _context.CategoryToProducts.Count(g => g.CategoryId == c.Id)
                      }).ToList();
        }
    }
}
