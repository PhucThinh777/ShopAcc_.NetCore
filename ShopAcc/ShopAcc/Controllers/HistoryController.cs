using Microsoft.AspNetCore.Mvc;

namespace ShopAcc.Controllers
{
    public class HistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
