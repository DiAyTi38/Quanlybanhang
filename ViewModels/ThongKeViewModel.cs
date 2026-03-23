using System;
using System.Linq;
using Quanlybanhang.DAO;

namespace Quanlybanhang.ViewModels
{
    public class ThongKeViewModel : BaseViewModel
    {
        public decimal DoanhThuNgay { get; set; }
        public decimal DoanhThuThang { get; set; }
        public int SoSanPhamBan { get; set; }
        public int TonKho { get; set; }

        public ThongKeViewModel()
        {
            var hoaDonDao = new HoaDonDAO();
            var prodDao = new ProductDAO();

            var hoaDons = hoaDonDao.GetAll();

            DoanhThuNgay = hoaDons
                .Where(h => h.NgayLap.Date == DateTime.Today)
                .Sum(h => h.TongTien);

            DoanhThuThang = hoaDons
                .Where(h => h.NgayLap.Month == DateTime.Now.Month && h.NgayLap.Year == DateTime.Now.Year)
                .Sum(h => h.TongTien);

            SoSanPhamBan = hoaDons.Sum(h => h.ChiTiet.Sum(ct => ct.SoLuong));

            TonKho = prodDao.GetAllProducts().Count;
        }
    }
}
