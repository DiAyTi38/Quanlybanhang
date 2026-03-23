using System.Data.SqlClient;

namespace Quanlybanhang.Data
{
    public class DatabaseHelper
    {
        private static string connectionString =
            "Data Source=.;Initial Catalog=QuanLyBanHang;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Try to open a connection and return result with message.
        public static (bool Success, string Message) TestConnection()
        {
            try
            {
                using var cn = GetConnection();
                cn.Open();
                return (true, "Kết nối database thành công.");
            }
            catch (System.Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}