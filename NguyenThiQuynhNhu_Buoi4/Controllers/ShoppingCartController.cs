using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NguyenThiQuynhNhu_Buoi4.Extensions;
using NguyenThiQuynhNhu_Buoi4.Models;
using NguyenThiQuynhNhu_Buoi4.Repositories;

namespace NguyenThiQuynhNhu_Buoi4.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ShoppingCartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Add sản phẩm vô giỏ hàng
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            // Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
            var product = await GetProductFromDatabase(productId);
            var cartItem = new CartItem
            {
                ProductId = productId,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity
            };
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            cart.AddItem(cartItem);
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// View giỏ hàng
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            return View(cart);
        }

        // Các actions khác...
        private async Task<Product> GetProductFromDatabase(int productId)
        {
            // Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm
            var product = await _productRepository.GetByIdAsync(productId);
            return product;
        }

        /// <summary>
        /// Remove sản phẩm khỏi giỏ hàng
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart is not null)
            {
                cart.RemoveItem(productId);
                // Lưu lại giỏ hàng vào Session sau khi đã xóa mục
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        ///CHECKOUT GIỎ HÀNG
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var productIds = cart.Items.Select(i => i.ProductId).ToList();
            var products = _context.Products.Where(p => productIds.Contains(p.Id)).ToDictionary(p => p.Id);

            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity),
                ApplicationUser = user,
                OrderDetails = cart.Items.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Product = products.ContainsKey(i.ProductId) ? products[i.ProductId] : null
                }).ToList()
            };

            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Bảo vệ chống CSRF
        public async Task<IActionResult> Checkout(Order order)
        {
            // Khai báo cart bên ngoài khối try-catch để sử dụng trong cả try và catch
            ShoppingCart cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

            try
            {
                if (cart == null || !cart.Items.Any())
                {
                    return RedirectToAction("Index");
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Gán các giá trị từ giỏ hàng và user
                order.UserId = user.Id;
                order.OrderDate = DateTime.UtcNow;
                order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
                order.OrderDetails = cart.Items.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList();

                // Lưu vào cơ sở dữ liệu
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Xóa giỏ hàng sau khi lưu thành công
                HttpContext.Session.Remove("Cart");

                // Chuyển sang trang OrderCompleted
                return View("OrderCompleted", order.Id);
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả lại view Checkout với thông báo lỗi
                ModelState.AddModelError("", $"Lỗi khi lưu đơn hàng: {ex.Message}");

                // Tải lại dữ liệu để hiển thị tóm tắt
                order.ApplicationUser = await _userManager.GetUserAsync(User);
                var productIds = cart?.Items.Select(i => i.ProductId).ToList() ?? new List<int>();
                var products = _context.Products.Where(p => productIds.Contains(p.Id)).ToDictionary(p => p.Id);
                order.OrderDetails = cart?.Items.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Product = products.ContainsKey(i.ProductId) ? products[i.ProductId] : null
                }).ToList() ?? new List<OrderDetail>();

                return View(order);
            }
        }
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null && quantity > 0)
            {
                item.Quantity = quantity; // Cập nhật số lượng mới
            }
            else if (item != null && quantity == 0)
            {
                cart.RemoveItem(productId); // Nếu số lượng = 0 thì xóa khỏi giỏ hàng
            }
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }
    }
}