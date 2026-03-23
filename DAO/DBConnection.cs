using System.Data.SqlClient;

public class DBConnection
{
    public static SqlConnection GetConnection()
    {
        string connectionString =
        "Server=localhost\\SQLEXPRESS;Database=QuanLyBanHang;Trusted_Connection=True;";

        return new SqlConnection(connectionString);
    }
}