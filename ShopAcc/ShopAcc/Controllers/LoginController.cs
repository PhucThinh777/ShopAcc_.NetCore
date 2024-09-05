using Microsoft.AspNetCore.Mvc;

namespace ShopAcc.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
