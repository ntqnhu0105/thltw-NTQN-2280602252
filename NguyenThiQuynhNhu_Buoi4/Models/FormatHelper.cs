
using System.Globalization;
namespace NguyenThiQuynhNhu_Buoi4.Models
{
    public static class FormatHelper
    {
        public static string ToVnd(this decimal amount)
        {
            return amount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")) + " VNĐ";
        }
    }
}
