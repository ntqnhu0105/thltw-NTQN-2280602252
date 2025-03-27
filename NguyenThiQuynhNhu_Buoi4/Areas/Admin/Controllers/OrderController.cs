using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NguyenThiQuynhNhu_Buoi4.Models;

namespace NguyenThiQuynhNhu_Buoi4.Areas.Admin.Controllers
{
    //PHAN VUNG`
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.Include(o => o.ApplicationUser).ToListAsync();
            return View(orders);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var order = await _context.Orders.Include(o => o.ApplicationUser).Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
    }
}
