using System.Collections.Generic;

namespace Loushop.Models
{
    public class Product
    {
    
        public int ItemId { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<CategoryToProduct> categoryToProducts { get; set; }
        public Item Item { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
