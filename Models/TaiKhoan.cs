public class TaiKhoan
{
    public int Id { get; set; }
    public string TenDangNhap { get; set; }
    public string MatKhau { get; set; }
    public string Quyen { get; set; }

    public TaiKhoan() { }

    public TaiKhoan(string user, string pass, string role)
    {
        TenDangNhap = user;
        MatKhau = pass;
        Quyen = role;
    }
}