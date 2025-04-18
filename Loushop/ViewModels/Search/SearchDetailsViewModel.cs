using Loushop.Models;
using Loushop.ViewModels.Search;
using System.Collections.Generic;

namespace Loushop.ViewModels
{
    public class SearchDetailsViewModel
    {
        public SearchProductViewModel Product { get; set; }
        public List<SearchCategoryViewModel> Categories { get; set; }
    }

}
