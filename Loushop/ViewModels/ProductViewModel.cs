using System.Collections.Generic;

namespace Loushop.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
    }

    public class CategoryViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

}
