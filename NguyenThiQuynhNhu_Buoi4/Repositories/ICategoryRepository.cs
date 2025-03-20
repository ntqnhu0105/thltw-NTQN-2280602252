using NguyenThiQuynhNhu_Buoi4.Models;

namespace NguyenThiQuynhNhu_Buoi4.Repositories
{
    public interface ICategoryRepository1
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);

    }
}
