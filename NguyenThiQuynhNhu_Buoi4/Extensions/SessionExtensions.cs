using System.Text.Json;

namespace NguyenThiQuynhNhu_Buoi4.Extensions
{
    public static class SessionExtensions
    {
        //Lớp dùng để quản lý thông tin lưu trữ cho từng user vào trang web
        //thông qua session
        //1000 user vào web => 1000 ss tương ứng để lưu
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        public static T GetObjectFromJson<T>(this ISession session,
       string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
