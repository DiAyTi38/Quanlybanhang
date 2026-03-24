
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using Quanlybanhang.Data;
using Quanlybanhang.Models;

namespace Quanlybanhang.DAO
{
    public class KhachHangDAO
    {
        // fallback in-memory data
        private static readonly List<KhachHang> _fallback = new List<KhachHang>
        {
            new KhachHang { MaKH = "KH01", TenKH = "Nguyễn Văn A", SoDienThoai = "0123456789" },
            new KhachHang { MaKH = "KH02", TenKH = "Trần Thị B", SoDienThoai = "0987654321" }
        };

        public List<KhachHang> GetAll()
        {
            try
            {
                using var cn = DatabaseHelper.GetConnection();
                using var cmd = new SqlCommand("SELECT MaKH, TenKH, SoDienThoai FROM KhachHang", cn);
                cn.Open();
                using var rd = cmd.ExecuteReader();
                var list = new List<KhachHang>();
                while (rd.Read())
                {
                    var kh = new KhachHang();
                    kh.MaKH = rd[0] == DBNull.Value ? string.Empty : rd.GetString(0);
                    kh.TenKH = rd[1] == DBNull.Value ? string.Empty : rd.GetString(1);
                    kh.SoDienThoai = rd[2] == DBNull.Value ? string.Empty : rd.GetString(2);
                    list.Add(kh);
                }
                return list;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetAll KhachHang error: " + ex.Message);
                return new List<KhachHang>(_fallback);
            }
        }

        public void AddKhachHang(KhachHang kh)
        {
            using var cn = DatabaseHelper.GetConnection();
            using var cmd = new SqlCommand("INSERT INTO KhachHang (MaKH, TenKH, SoDienThoai) VALUES (@ma,@ten,@sdt)", cn);
            cmd.Parameters.AddWithValue("@ma", kh.MaKH ?? string.Empty);
            cmd.Parameters.AddWithValue("@ten", kh.TenKH ?? string.Empty);
            cmd.Parameters.AddWithValue("@sdt", kh.SoDienThoai ?? string.Empty);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateKhachHang(KhachHang kh)
        {
            using var cn = DatabaseHelper.GetConnection();
            using var cmd = new SqlCommand("UPDATE KhachHang SET TenKH = @ten, SoDienThoai = @sdt WHERE MaKH = @ma", cn);
            cmd.Parameters.AddWithValue("@ten", kh.TenKH ?? string.Empty);
            cmd.Parameters.AddWithValue("@sdt", kh.SoDienThoai ?? string.Empty);
            cmd.Parameters.AddWithValue("@ma", kh.MaKH ?? string.Empty);
            cn.Open();
            var rows = cmd.ExecuteNonQuery();

            if (rows == 0)
            {
                throw new Exception("Không tìm thấy Khách Hàng. Có thể bạn đã sửa Mã KH hoặc khách đã bị xoá.");
            }
        }

        // Delete customer by MaKH; returns true if DB delete succeeded
        public bool DeleteKhachHang(string maKH)
        {
            using var cn = DatabaseHelper.GetConnection();
            using var cmd = new SqlCommand("DELETE FROM KhachHang WHERE MaKH = @ma", cn);
            cmd.Parameters.AddWithValue("@ma", maKH ?? string.Empty);
            cn.Open();
            var rows = cmd.ExecuteNonQuery();

            return rows > 0;
        }
    }
}