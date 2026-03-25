using System;
using System.Data.SqlClient;
using Quanlybanhang.Data;

namespace Quanlybanhang.DAO
{
    public class DoanhThuDAO
    {
        public DoanhThuDAO()
        {
            EnsureTableExists();
        }

        private void EnsureTableExists()
        {
            try
            {
                using var cn = DatabaseHelper.GetConnection();
                string query = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='DOANH_THU' and xtype='U')
                    CREATE TABLE DOANH_THU (
                        Ngay DATE PRIMARY KEY,
                        TongDoanhThu DECIMAL(18,2) NOT NULL DEFAULT 0
                    )";
                using var cmd = new SqlCommand(query, cn);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EnsureTableExists error: " + ex.Message);
            }
        }

        public void AddDoanhThu(DateTime ngay, decimal soTien)
        {
            try
            {
                using var cn = DatabaseHelper.GetConnection();
                string query = @"
                    IF EXISTS (SELECT 1 FROM DOANH_THU WHERE Ngay = @ngay)
                    BEGIN
                        UPDATE DOANH_THU
                        SET TongDoanhThu = TongDoanhThu + @tien
                        WHERE Ngay = @ngay
                    END
                    ELSE
                    BEGIN
                        INSERT INTO DOANH_THU (Ngay, TongDoanhThu)
                        VALUES (@ngay, @tien)
                    END";

                using var cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@ngay", ngay.Date);
                cmd.Parameters.AddWithValue("@tien", soTien);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("AddDoanhThu error: " + ex.Message);
            }
        }
    }
}
