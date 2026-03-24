using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; // ✅ đổi sang cái này
using System.Linq;
using Quanlybanhang.Data;
using Quanlybanhang.Models;

namespace Quanlybanhang.DAO
{
    public class ProductDAO
    {
        // Fallback static list used when DB access fails
        private static readonly List<Product> _fallback = new List<Product>
        {
            new Product { Id = "SH01", Name = "Áo khoác da Vintage", Price = 350000, IsNew = false, Status = "Còn hàng" },
            new Product { Id = "SH02", Name = "Giày thể thao Nike Air", Price = 800000, IsNew = false, Status = "Còn hàng" },
            new Product { Id = "NEW01", Name = "Áo thun Local Brand", Price = 250000, IsNew = true, Status = "Còn hàng" }
        };

        public List<Product> GetAllProducts()
        {
            try
            {
                using var cn = DatabaseHelper.GetConnection();
                using var cmd = new SqlCommand("SELECT Id, Name, Price, IsNew, Description, Status FROM Products", cn);
                cn.Open();
                using var rd = cmd.ExecuteReader();
                var list = new List<Product>();
                while (rd.Read())
                {
                    var p = new Product();
                    p.Id = rd[0] == DBNull.Value ? string.Empty : rd.GetString(0);
                    p.Name = rd[1] == DBNull.Value ? string.Empty : rd.GetString(1);
                    p.Price = rd[2] == DBNull.Value ? 0 : rd.GetDecimal(2);
                    p.IsNew = rd[3] == DBNull.Value ? false : rd.GetBoolean(3);
                    p.Description = rd[4] == DBNull.Value ? string.Empty : rd.GetString(4);
                    p.Status = rd[5] == DBNull.Value ? string.Empty : rd.GetString(5);
                    list.Add(p);
                }
                return list;
            }
            catch (Exception)
            {
                return _fallback.ToList();
            }
        }

        public List<Product> GetAvailableProducts()
        {
            try
            {
                var all = GetAllProducts();
                return all.Where(p => string.Equals(p.Status, "Còn hàng", StringComparison.OrdinalIgnoreCase)).ToList();
            }
            catch
            {
                return _fallback.Where(p => p.Status == "Còn hàng").ToList();
            }
        }

        // ✅ ✅ ✅ THÊM HÀM NÀY (FIX LỖI CHÍNH)
        public bool AddProduct(Product product)
        {
            using var cn = DatabaseHelper.GetConnection();
            string query = @"INSERT INTO Products (Id, Name, Price, IsNew, Description, Status)
                             VALUES (@Id, @Name, @Price, @IsNew, @Description, @Status)";

            using var cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@Id", product.Id ?? string.Empty);
            cmd.Parameters.AddWithValue("@Name", product.Name ?? string.Empty);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@IsNew", product.IsNew);
            cmd.Parameters.AddWithValue("@Description", product.Description ?? string.Empty);
            cmd.Parameters.AddWithValue("@Status", product.Status ?? "Còn hàng");

            cn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
        public bool DeleteProduct(string id)
        {
            using var cn = DatabaseHelper.GetConnection();

            string query = "DELETE FROM Products WHERE Id = @Id";

            using var cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@Id", id ?? string.Empty);

            cn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public void UpdateProductStatus(string productId, string newStatus)
        {
            using var cn = DatabaseHelper.GetConnection();
            using var cmd = new SqlCommand("UPDATE Products SET Status = @st WHERE Id = @id", cn);
            cmd.Parameters.AddWithValue("@st", newStatus ?? string.Empty);
            cmd.Parameters.AddWithValue("@id", productId ?? string.Empty);
            cn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}