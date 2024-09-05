using Microsoft.AspNetCore.Mvc;

namespace ShopAcc.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
