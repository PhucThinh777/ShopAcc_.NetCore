using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAcc.Models;
using ShopAcc.Reponsitory;

namespace ShopAcc.Controllers
{
    public class ServerController : Controller
    {
        private readonly DataContext _dataContext;
        public ServerController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Index(string Slug ="")
        {
            ServerModel servers = _dataContext.Servers.Where(c=>c.Slug == Slug).FirstOrDefault();
            if(servers == null)
            {
                return RedirectToAction("Index");
            }

            var nickBySever = _dataContext.Nicks.Where(p => p.ServerId == servers.Id);
            return View(await nickBySever.OrderByDescending(p=>p.Id).ToListAsync());
        }
    }
}
