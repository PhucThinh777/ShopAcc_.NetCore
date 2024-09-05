using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAcc.Models;
using ShopAcc.Reponsitory;

namespace ShopAcc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServerController : Controller
    {
        private readonly DataContext _dataContext;
        public ServerController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Servers.OrderByDescending(p => p.Id).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int Id)
        {
            ServerModel server = await _dataContext.Servers.FindAsync(Id);
            return View(server);
        }

        #region Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServerModel server)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Gán giá trị cho Slug
                    server.Slug = server.Name.Replace(" ", "-").ToLower();

                    // Kiểm tra trùng lặp Slug
                    //var existingProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                    //if (existingProduct != null)
                    //{
                    //    ModelState.AddModelError("", "Danh mục đã có trong database");
                    //    return View(category);
                    //}

                    // Lưu đối tượng vào cơ sở dữ liệu
                    _dataContext.Add(server);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Thêm server thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"Đã xảy ra lỗi khi thêm server {ex.Message}";

                    if (ex.InnerException != null)
                    {
                        TempData["error"] += $" Inner exception: {ex.InnerException.Message}";
                    }

                    return View(server);
                }
            }
            else
            {
                TempData["error"] = "Model đang thiếu";
                return View(server);
            }
        }
        #endregion

        #region Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServerModel server)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Gán giá trị cho Slug
                    server.Slug = server.Name.Replace(" ", "-").ToLower();

                    // Kiểm tra trùng lặp Slug
                    //var existingProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                    //if (existingProduct != null)
                    //{
                    //    ModelState.AddModelError("", "Danh mục đã có trong database");
                    //    return View(category);
                    //}

                    // Lưu đối tượng vào cơ sở dữ liệu
                    _dataContext.Update(server);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Sửa server thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"Đã xảy ra lỗi khi sửa server {ex.Message}";

                    if (ex.InnerException != null)
                    {
                        TempData["error"] += $" Inner exception: {ex.InnerException.Message}";
                    }

                    return View(server);
                }
            }
            else
            {
                TempData["error"] = "Model đang thiếu";
                return View(server);
            }
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int Id)
        {
            ServerModel server = await _dataContext.Servers.FindAsync(Id);
            _dataContext.Servers.Remove(server);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Server đã xoá";
            return RedirectToAction("Index");
        }
        #endregion
    }
}
