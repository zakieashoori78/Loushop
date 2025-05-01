using Loushop.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Loushop.Components
{
    public class ProductGroupsComponents : ViewComponent
    {
       private IGroupRepository _groupRepository;

        public ProductGroupsComponents(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("/Views/Components/ProductGroupsComponent.cshtml",model: _groupRepository.GetGroupForShow());
        }

      
    }
}

