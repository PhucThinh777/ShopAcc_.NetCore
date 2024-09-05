using Microsoft.AspNetCore.Mvc;
using ShopAcc.Reponsitory;

namespace ShopAcc.Controllers
{
    public class NickController : Controller
    {
        private readonly DataContext _dataContext;
        public NickController(DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int Id)
        {
            if(Id == null)
            {
                return RedirectToAction("Index");
            }

            var nickById = _dataContext.Nicks.Where(c=>c.Id == Id).FirstOrDefault();

            return View(nickById);
        }
    }
}
