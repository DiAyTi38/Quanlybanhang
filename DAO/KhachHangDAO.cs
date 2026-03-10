using System.Collections.Generic;
using Quanlybanhang.Models;

namespace Quanlybanhang.DAO
{
    public class KhachHangDAO
    {
        private static List<KhachHang> _khachHangs = new List<KhachHang>
        {
            new KhachHang { MaKH = "KH01", TenKH = "Nguyễn Văn A", SoDienThoai = "0123456789" },
            new KhachHang { MaKH = "KH02", TenKH = "Trần Thị B", SoDienThoai = "0987654321" }
        };

        public List<KhachHang> GetAll()
        {
            return _khachHangs;
        }

        public void AddKhachHang(KhachHang kh)
        {
            _khachHangs.Add(kh);
        }
    }
}
