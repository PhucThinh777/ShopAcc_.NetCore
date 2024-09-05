using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopAcc.Models;
using ShopAcc.Reponsitory;

namespace ShopAcc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CharacterController : Controller
    {
        private readonly DataContext _dataContext;
        public CharacterController(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Characters.OrderByDescending(p => p.Id).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int Id)
        {
            CharacterModel character = await _dataContext.Characters.FindAsync(Id);
            return View(character);
        }

        #region Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CharacterModel character)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Gán giá trị cho Slug
                    character.Slug = character.Name.Replace(" ", "-").ToLower();

                    // Kiểm tra trùng lặp Slug
                    //var existingProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                    //if (existingProduct != null)
                    //{
                    //    ModelState.AddModelError("", "Danh mục đã có trong database");
                    //    return View(category);
                    //}

                    // Lưu đối tượng vào cơ sở dữ liệu
                    _dataContext.Add(character);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Thêm character thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"Đã xảy ra lỗi khi thêm character {ex.Message}";

                    if (ex.InnerException != null)
                    {
                        TempData["error"] += $" Inner exception: {ex.InnerException.Message}";
                    }

                    return View(character);
                }
            }
            else
            {
                TempData["error"] = "Model đang thiếu";
                return View(character);
            }
        }
        #endregion

        #region Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CharacterModel character)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Gán giá trị cho Slug
                    character.Slug = character.Name.Replace(" ", "-").ToLower();

                    // Kiểm tra trùng lặp Slug
                    //var existingProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.Slug == category.Slug);
                    //if (existingProduct != null)
                    //{
                    //    ModelState.AddModelError("", "Danh mục đã có trong database");
                    //    return View(category);
                    //}

                    // Lưu đối tượng vào cơ sở dữ liệu
                    _dataContext.Update(character);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Sửa character thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"Đã xảy ra lỗi khi sửa character {ex.Message}";

                    if (ex.InnerException != null)
                    {
                        TempData["error"] += $" Inner exception: {ex.InnerException.Message}";
                    }

                    return View(character);
                }
            }
            else
            {
                TempData["error"] = "Model đang thiếu";
                return View(character);
            }
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int Id)
        {
            CharacterModel character = await _dataContext.Characters.FindAsync(Id);
            _dataContext.Characters.Remove(character);
            await _dataContext.SaveChangesAsync();
            TempData["error"] = "Character đã xoá";
            return RedirectToAction("Index");
        }
        #endregion
    }
}
