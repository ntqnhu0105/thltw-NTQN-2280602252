namespace NguyenThiQuynhNhu_Buoi4.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        // Foreign key
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
