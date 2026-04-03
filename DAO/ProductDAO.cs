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
                    p.Id = rd[0] == DBNull.Value ? string.Empty : rd[0].ToString();
                    p.Name = rd[1] == DBNull.Value ? string.Empty : rd[1].ToString();
                    p.Price = rd[2] == DBNull.Value ? 0 : Convert.ToDecimal(rd[2]);
                    p.IsNew = rd[3] == DBNull.Value ? false : Convert.ToBoolean(rd[3]);
                    p.Description = rd[4] == DBNull.Value ? string.Empty : rd[4].ToString();
                    p.Status = rd[5] == DBNull.Value ? string.Empty : rd[5].ToString();
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
            string query = @"INSERT INTO Products (Name, Price, IsNew, Description, Status)
                             VALUES (@Name, @Price, @IsNew, @Description, @Status)";

            using var cmd = new SqlCommand(query, cn);
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
            try
            {
                using var cn = DatabaseHelper.GetConnection();

                string query = "DELETE FROM Products WHERE Id = @Id";

                using var cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@Id", id ?? string.Empty);

                cn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch
            {
                // fallback: xóa trong list tạm
                var item = _fallback.FirstOrDefault(p => p.Id == id);
                if (item != null)
                {
                    _fallback.Remove(item);
                }
                return true;
            }
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