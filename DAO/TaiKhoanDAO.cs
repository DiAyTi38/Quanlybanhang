using System;
using System.Data.SqlClient;
using System.Diagnostics;
using Quanlybanhang.Data;

namespace Quanlybanhang.DAO
{
    public class TaiKhoanDAO
    {
        public TaiKhoan DangNhap(string user, string pass)
        {
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
                return null;

            user = user.Trim();
            pass = pass.Trim();

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT username, [password], vai_tro FROM TAI_KHOAN WHERE username = @user AND [password] = @pass AND trang_thai = 'hoat_dong'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", user);
                        cmd.Parameters.AddWithValue("@pass", pass);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TaiKhoan tk = new TaiKhoan();
                                tk.TenDangNhap = reader["username"]?.ToString();
                                tk.MatKhau = reader["password"]?.ToString();
                                tk.Quyen = reader["vai_tro"]?.ToString();
                                return tk;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DangNhap error: " + ex.Message);
            }

            return null;
        }
    }
}