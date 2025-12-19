using Loushop.Models;
using System.Collections.Generic;

namespace Loushop.ViewModels.Search
{
    public class SearchViewModel
    {
        public string Query { get; set; }
        public List<Product> Results { get; set; }
    }

}
