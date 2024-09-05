using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAcc.Models;
using ShopAcc.Reponsitory;

namespace ShopAcc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly DataContext _dataContext;
        public CategoryController(DataContext context)
        {
            _dataContext = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Categories.OrderByDescending(p => p.Id).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
            return View(category);
        }

        #region Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Gán giá trị cho Slug
                    category.Slug = category.Name.Replace(" ", "-").ToLower();

                    // Kiểm tra trùng lặp Slug
                    //var existingProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                    //if (existingProduct != null)
                    //{
                    //    ModelState.AddModelError("", "Danh mục đã có trong database");
                    //    return View(category);
                    //}

                    // Lưu đối tượng vào cơ sở dữ liệu
                    _dataContext.Add(category);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Thêm danh mục thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"Đã xảy ra lỗi khi thêm danh mục: {ex.Message}";

                    if (ex.InnerException != null)
                    {
                        TempData["error"] += $" Inner exception: {ex.InnerException.Message}";
                    }

                    return View(category);
                }
            }
            else
            {
                TempData["error"] = "Model đang thiếu";
                return View(category);
            }
        }
        #endregion

        #region Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Gán giá trị cho Slug
                    category.Slug = category.Name.Replace(" ", "-").ToLower();

                    // Kiểm tra trùng lặp Slug
                    //var existingProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                    //if (existingProduct != null)
                    //{
                    //    ModelState.AddModelError("", "Danh mục đã có trong database");
                    //    return View(category);
                    //}

                    // Lưu đối tượng vào cơ sở dữ liệu
                    _dataContext.Update(category);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Sửa danh mục thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"Đã xảy ra lỗi khi sửa danh mục: {ex.Message}";

                    if (ex.InnerException != null)
                    {
                        TempData["error"] += $" Inner exception: {ex.InnerException.Message}";
                    }

                    return View(category);
                }
            }
            else
            {
                TempData["error"] = "Model đang thiếu";
                return View(category);
            }
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int Id)
        {
            CategoryModel category = await _dataContext.Categories.FindAsync(Id);
            _dataContext.Categories.Remove(category);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Danh mục đã xoá";
            return RedirectToAction("Index");
        }
        #endregion
    }
}
