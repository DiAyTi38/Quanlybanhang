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
            var dtDao = new DoanhThuDAO();

            // Lấy trực tiếp từ Database DOANH_THU thay vì RAM
            DoanhThuNgay = dtDao.GetDoanhThuNgay(DateTime.Today);
            DoanhThuThang = dtDao.GetDoanhThuThang(DateTime.Now.Month, DateTime.Now.Year);

            // Số SP bán được lấy từ RAM HoaDonDAO tạm, lý tưởng nên đếm CTHoaDon trong DB
            var hoaDons = hoaDonDao.GetAll();
            SoSanPhamBan = hoaDons.Sum(h => h.ChiTiet.Sum(ct => ct.SoLuong));

            TonKho = prodDao.GetAllProducts()
                .Where(p => p.Status != "Đã bán").Count();
        }
    }
}
