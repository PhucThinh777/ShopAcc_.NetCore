using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ShopAcc.Reponsitory.Components
{
    public class ServersViewComponent : ViewComponent
    {
        private readonly DataContext _dataContext;

        public ServersViewComponent(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IViewComponentResult> InvokeAsync () => View(await _dataContext.Servers.ToListAsync());
    }
}
