using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loushop.Models
{
    public class Item
    {
        public int Id { get; set; }
        public Decimal Price { get; set; }
        public int QuantityInStocke { get; set;}


        public Product Product { get; set; }
    }


}
