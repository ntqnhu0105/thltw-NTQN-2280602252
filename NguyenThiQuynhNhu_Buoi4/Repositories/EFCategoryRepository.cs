using Microsoft.EntityFrameworkCore;
using NguyenThiQuynhNhu_Buoi4.Models;

namespace NguyenThiQuynhNhu_Buoi4.Repositories
{
    public class EFCategoryRepository1 : ICategoryRepository1
    {
        private readonly ApplicationDbContext _context;
        public EFCategoryRepository1(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GetAllAsync: Lấy tất cả danh mục trong (Category)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            // return await _context.Products.ToListAsync();
            return await _context.Categories.Include(p => p.Products) // Include thông tin về product
            .ToListAsync();
        }
        /// <summary>
        /// GetByIdAsync: Lấy danh mục theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Categories.Include(p => p.Products).FirstOrDefaultAsync(p => p.Id == id);
        }
        /// <summary>
        /// AddAsync: Thêm danh mục
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// UpdateAsync: Cập nhật danh mục
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// DeleteAsync: Xóa danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
