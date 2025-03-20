using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NguyenThiQuynhNhu_Buoi4.Models;
using NguyenThiQuynhNhu_Buoi4.Repositories;
//CUSTOMER CHỈ ĐƯỢC PHÉP VIEW LIST VÀ DETAIL!!!!!!
namespace NguyenThiQuynhNhu_Buoi4.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = SD.Role_Customer)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository1 _categoryRepository;
        public ProductController(IProductRepository productRepository, ICategoryRepository1 categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
