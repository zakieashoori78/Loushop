using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loushop.Models
{
    public class CategoryToProduct
    {

        public int CategoryId { get; set; }
        public int ProductId { get; set; }

   

        public Category Category { get; set; }
        public Product Product { get; set; }

    }
}
