using System.ComponentModel.DataAnnotations;

namespace NguyenThiQuynhNhu_Buoi4.Models
{
    public class Product
    {
        public int Id { get; set; }
        // yêu cầu bắt buộc có chữ Required
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public List<ProductImage>? Images { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
