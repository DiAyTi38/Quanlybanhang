using System.Collections.Generic;
using System.Linq;
using Quanlybanhang.Models;

namespace Quanlybanhang.DAO
{
    public class ProductDAO
    {
        // Mock DB: Dùng lại danh sách tĩnh cho toàn app (trong thực tế sẽ query DB)
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = "SH01", Name = "Áo khoác da Vintage", Price = 350000, IsNew = false, Status = "Còn hàng" },
            new Product { Id = "SH02", Name = "Giày thể thao Nike Air", Price = 800000, IsNew = false, Status = "Còn hàng" },
            new Product { Id = "NEW01", Name = "Áo thun Local Brand", Price = 250000, IsNew = true, Status = "Còn hàng" }
        };

        public List<Product> GetAllProducts()
        {
            return _products;
        }

        // Lấy các sản phẩm chưa bán để đưa vào danh sách chọn khi lập Hóa Đơn
        public List<Product> GetAvailableProducts()
        {
            return _products.Where(p => p.Status == "Còn hàng").ToList();
        }

        public void UpdateProductStatus(string productId, string newStatus)
        {
            var product = _products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                product.Status = newStatus;
                // Nếu lưu DB thì gọi SaveChanges ở đây
            }
        }
    }
}
