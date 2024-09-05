using Microsoft.AspNetCore.Mvc;
using ShopAcc.Reponsitory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;


using ShopAcc.Models;


namespace ShopAcc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NickController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public NickController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Nicks.OrderByDescending(p => p.Id).Include(p => p.Server).Include(p => p.Category).Include(p => p.Character).ToListAsync());

        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Servers = new SelectList(_dataContext.Servers, "Id", "Name");
            ViewBag.Characters = new SelectList(_dataContext.Characters, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NickModel nick)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", nick.CategoryId);
            ViewBag.Servers = new SelectList(_dataContext.Servers, "Id", "Name", nick.ServerId);
            ViewBag.Characters = new SelectList(_dataContext.Characters, "Id", "Name", nick.CharacterId);

            if (ModelState.IsValid)
            {
                try
                {
                    List<string> tuongNoiBatFileNames = new List<string>();
                    List<string> tranNoiBatFileNames = new List<string>();

                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/nicks");

                    // Xử lý ảnh tướng nổi bật
                    if (nick.UploadTuongNoiBat != null && nick.UploadTuongNoiBat.Length > 0)
                    {
                        if (!Directory.Exists(uploadsDir))
                        {
                            Directory.CreateDirectory(uploadsDir);
                        }

                        foreach (var file in nick.UploadTuongNoiBat)
                        {
                            string extension = Path.GetExtension(file.FileName).TrimStart('.').ToLower();
                            string fileName = "Tran" + Path.GetFileNameWithoutExtension(file.FileName) + "." + extension;
                            string filePath = Path.Combine(uploadsDir, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            tuongNoiBatFileNames.Add(fileName);
                        }

                        nick.TuongNoiBat = string.Join(",", tuongNoiBatFileNames); // Lưu danh sách tệp dưới dạng chuỗi phân cách bởi dấu phẩy
                    }

                    // Xử lý ảnh trấn nổi bật
                    if (nick.UploadTranNoiBat != null && nick.UploadTranNoiBat.Length > 0)
                    {
                        if (!Directory.Exists(uploadsDir))
                        {
                            Directory.CreateDirectory(uploadsDir);
                        }

                        foreach (var file in nick.UploadTranNoiBat)
                        {
                            string extension = Path.GetExtension(file.FileName).TrimStart('.').ToLower();
                            string fileName = "Tran" + Path.GetFileNameWithoutExtension(file.FileName) + "." + extension;
                            string filePath = Path.Combine(uploadsDir, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            tranNoiBatFileNames.Add(fileName);
                        }

                        nick.TranNoiBat = string.Join(",", tranNoiBatFileNames); // Lưu danh sách tệp dưới dạng chuỗi phân cách bởi dấu phẩy
                    }

                    // Xử lý ảnh bìa
                    if (nick.UploadAnhBia != null)
                    {
                        string extension = Path.GetExtension(nick.UploadAnhBia.FileName).TrimStart('.').ToLower();
                        string fileName = Guid.NewGuid().ToString() + "." + extension;
                        string filePath = Path.Combine(uploadsDir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await nick.UploadAnhBia.CopyToAsync(stream);
                        }

                        nick.AnhBia = fileName; // Lưu tên tệp vào thuộc tính
                    }

                    // Lưu đối tượng vào cơ sở dữ liệu
                    _dataContext.Add(nick);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Thêm nick thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"Đã xảy ra lỗi khi thêm nick: {ex.Message}";
                    if (ex.InnerException != null)
                    {
                        TempData["error"] += $" Inner exception: {ex.InnerException.Message}";
                    }

                    return View(nick);
                }
            }
            else
            {
                // Lấy tất cả các lỗi từ ModelState
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                foreach (var error in errors)
                {
                    TempData["error"] += error.ErrorMessage + "\n"; // Ghi log tất cả các lỗi vào TempData hoặc một nơi lưu trữ khác
                }

                TempData["error"] += "Model không hợp lệ.";
                return View(nick);
            }
        }

        public async Task<IActionResult> Edit(int Id)
        {
            NickModel nick = await _dataContext.Nicks.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", nick.CategoryId);
            ViewBag.Servers = new SelectList(_dataContext.Servers, "Id", "Name", nick.ServerId);
            ViewBag.Characters = new SelectList(_dataContext.Characters, "Id", "Name", nick.CharacterId);

            return View(nick);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NickModel nick)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", nick.CategoryId);
            ViewBag.Servers = new SelectList(_dataContext.Servers, "Id", "Name", nick.ServerId);
            ViewBag.Characters = new SelectList(_dataContext.Characters, "Id", "Name", nick.CharacterId);

            if (ModelState.IsValid)
            {
                try
                {
                    var existingNick = await _dataContext.Nicks.FindAsync(nick.Id);

                    if (existingNick == null)
                    {
                        TempData["error"] = "Nick không tồn tại.";
                        return RedirectToAction("Index");
                    }

                    List<string> tuongNoiBatFileNames = new List<string>();
                    List<string> tranNoiBatFileNames = new List<string>();
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/nicks");

                    // Xử lý ảnh tướng nổi bật
                    if (nick.UploadTuongNoiBat != null && nick.UploadTuongNoiBat.Length > 0)
                    {
                        if (!Directory.Exists(uploadsDir))
                        {
                            Directory.CreateDirectory(uploadsDir);
                        }

                        foreach (var file in nick.UploadTuongNoiBat)
                        {
                            string extension = Path.GetExtension(file.FileName).TrimStart('.').ToLower();
                            string fileName = "Tuong" + Path.GetFileNameWithoutExtension(file.FileName) + "." + extension;
                            string filePath = Path.Combine(uploadsDir, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            tuongNoiBatFileNames.Add(fileName);
                        }

                        existingNick.TuongNoiBat = string.Join(",", tuongNoiBatFileNames);
                    }

                    // Xử lý ảnh trấn nổi bật
                    if (nick.UploadTranNoiBat != null && nick.UploadTranNoiBat.Length > 0)
                    {
                        if (!Directory.Exists(uploadsDir))
                        {
                            Directory.CreateDirectory(uploadsDir);
                        }

                        foreach (var file in nick.UploadTranNoiBat)
                        {
                            string extension = Path.GetExtension(file.FileName).TrimStart('.').ToLower();
                            string fileName = "Tran" + Path.GetFileNameWithoutExtension(file.FileName) + "." + extension;
                            string filePath = Path.Combine(uploadsDir, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            tranNoiBatFileNames.Add(fileName);
                        }

                        existingNick.TranNoiBat = string.Join(",", tranNoiBatFileNames);
                    }

                    // Xử lý ảnh bìa
                    if (nick.UploadAnhBia != null)
                    {
                        string extension = Path.GetExtension(nick.UploadAnhBia.FileName).TrimStart('.').ToLower();
                        string fileName = Guid.NewGuid().ToString() + "." + extension;
                        string filePath = Path.Combine(uploadsDir, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await nick.UploadAnhBia.CopyToAsync(stream);
                        }

                        // Xóa ảnh bìa cũ
                        if (!string.IsNullOrEmpty(existingNick.AnhBia))
                        {
                            string oldFile = Path.Combine(uploadsDir, existingNick.AnhBia);
                            if (System.IO.File.Exists(oldFile))
                            {
                                System.IO.File.Delete(oldFile);
                            }
                        }

                        existingNick.AnhBia = fileName;
                    }

                    // Cập nhật các thuộc tính khác
                    existingNick.LevelAcc = nick.LevelAcc;
                    existingNick.Description = nick.Description;
                    existingNick.Price = nick.Price;
                    existingNick.CategoryId = nick.CategoryId;
                    existingNick.ServerId = nick.ServerId;
                    existingNick.CharacterId = nick.CharacterId;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _dataContext.Update(existingNick);
                    await _dataContext.SaveChangesAsync();
                    TempData["success"] = "Cập nhật nick thành công";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = $"Đã xảy ra lỗi khi cập nhật nick: {ex.Message}";
                    if (ex.InnerException != null)
                    {
                        TempData["error"] += $" Inner exception: {ex.InnerException.Message}";
                    }

                    return View(nick);
                }
            }
            else
            {
                TempData["error"] = "Model không hợp lệ.";
                return View(nick);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            NickModel nick = await _dataContext.Nicks.FindAsync(Id);
            if (nick == null)
            {
                TempData["error"] = "Nick không tồn tại.";
                return RedirectToAction("Index");
            }

            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/nicks");

            // Xóa ảnh bìa nếu có
            if (!string.IsNullOrEmpty(nick.AnhBia))
            {
                string filePath = Path.Combine(uploadsDir, nick.AnhBia);
                try
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi xóa ảnh bìa.");
                }
            }

            // Xóa ảnh tướng nổi bật nếu có
            if (!string.IsNullOrEmpty(nick.TuongNoiBat))
            {
                string[] tuongFiles = nick.TuongNoiBat.Split(",");
                foreach (var fileName in tuongFiles)
                {
                    string filePath = Path.Combine(uploadsDir, fileName);
                    try
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi xóa tệp ảnh tướng nổi bật.");
                    }
                }
            }

            // Xóa ảnh trấn nổi bật nếu có
            if (!string.IsNullOrEmpty(nick.TranNoiBat))
            {
                string[] tranFiles = nick.TranNoiBat.Split(",");
                foreach (var fileName in tranFiles)
                {
                    string filePath = Path.Combine(uploadsDir, fileName);
                    try
                    {
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi xóa tệp ảnh trấn nổi bật.");
                    }
                }
            }

            // Xóa đối tượng Nick khỏi cơ sở dữ liệu
            _dataContext.Nicks.Remove(nick);
            await _dataContext.SaveChangesAsync();

            TempData["success"] = "Nick đã được xóa thành công.";
            return RedirectToAction("Index");
        }
    }
}
