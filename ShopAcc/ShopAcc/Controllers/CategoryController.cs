using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAcc.Models;
using ShopAcc.Reponsitory;

namespace ShopAcc.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        public CategoryController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index(string Slug ="")
        {
            CategoryModel category = _dataContext.Categories.Where(c=>c.Slug ==Slug).FirstOrDefault();
            if (category == null) 
            {
                return RedirectToAction("Index");
            }

            var nickByCategory = _dataContext.Nicks.Where(p => p.CategoryId == category.Id);
            return View(await nickByCategory.OrderByDescending(p=>p.Id).ToListAsync());
        }
    }
}
