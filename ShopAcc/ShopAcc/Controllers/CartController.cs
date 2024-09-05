using Microsoft.AspNetCore.Mvc;
using ShopAcc.Models.ViewModels;
using ShopAcc.Models;
using ShopAcc.Reponsitory;

namespace ShopAcc.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;
        public CartController(DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Index()
        {
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
            };
            return View(cartVM);
        }

        public async Task<IActionResult> Add(int Id)
        {
            NickModel nick = await _dataContext.Nicks.FindAsync(Id);
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItems = cart.Where(c => c.NickId == Id).FirstOrDefault();

            if (cartItems == null)
            {
                cart.Add(new CartItemModel(nick));
            }
            else
            {
                cartItems.Quantity += 1;
            }
            //Lưu trữ vào session
            HttpContext.Session.SetJson("Cart", cart);

            //Dùng biến tạm để thông báo
            TempData["success"] = "Sản phẩm đã được thêm vào giỏ hàng";

            //Trả về trang hiện tại
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Decrease(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            CartItemModel cartItem = cart.Where(c => c.NickId == Id).FirstOrDefault();
            if (cartItem.Quantity >= 1)
            {
                --cartItem.Quantity;

            }
            else
            {
                cart.RemoveAll(p => p.NickId == Id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            //Dùng biến tạm để thông báo
            TempData["success"] = "Giảm số lượng";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Increase(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            CartItemModel cartItem = cart.Where(c => c.NickId == Id).FirstOrDefault();
            if (cartItem.Quantity >= 1)
            {
                ++cartItem.Quantity;

            }
            else
            {
                cart.RemoveAll(p => p.NickId == Id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            TempData["success"] = "Tăng số lượng";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
            cart.RemoveAll(p => p.NickId == Id);
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            TempData["success"] = "Xoá sản phẩm thành công";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clear()
        {
            HttpContext.Session.Remove("Cart");
            TempData["success"] = "Giỏ hàng của bạn đã trống";
            return RedirectToAction("Index");
        }
    }
}
