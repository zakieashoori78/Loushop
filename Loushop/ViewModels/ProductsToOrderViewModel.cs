using Loushop.Models;
using System.Collections.Generic;

namespace Loushop.ViewModels
{
    public class ProductsToOrderViewModel
    {
        public List<Product> Products { get; set; }
        public Order? Order { get; set; }
    }
}
