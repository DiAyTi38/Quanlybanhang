using System;
using System.Collections.Generic;
using Quanlybanhang.Models;

namespace Quanlybanhang.DAO
{
    public class HoaDonDAO
    {
        private static List<HoaDon> _danhSachHD = new List<HoaDon>();
        private readonly ProductDAO _productDAO = new ProductDAO();

        public HoaDonDAO()
        {
            EnsureMockData();
        }

        private void EnsureMockData()
        {
            if (_danhSachHD.Count > 0) return;

            var khDao = new KhachHangDAO();
            var khachHangs = khDao.GetAll();
            var products = _productDAO.GetAllProducts();
            if (products.Count == 0 || khachHangs.Count == 0) return;

            var rnd = new Random();
            int hdMa = 1;

            foreach (var kh in khachHangs)
            {
                // Mỗi khách hàng tạo khoảng 2 hoá đơn
                for (int i = 0; i < 2; i++)
                {
                    var hd = new HoaDon
                    {
                        MaHD = $"HD{hdMa:D3}",
                        KhachHang = kh,
                        NgayLap = DateTime.Now.AddDays(-rnd.Next(1, 30))
                    };
                    hdMa++;

                    // Mỗi hoá đơn có 1-2 sản phẩm
                    int productCount = rnd.Next(1, 3);
                    decimal total = 0;
                    for (int j = 0; j < productCount; j++)
                    {
                        var product = products[rnd.Next(products.Count)];
                        var ct = new CTHoaDon
                        {
                            MaHD = hd.MaHD,
                            SanPham = product,
                            SoLuong = rnd.Next(1, 4),
                            DonGia = product.Price
                        };
                        hd.ChiTiet.Add(ct);
                        total += ct.ThanhTien;
                    }

                    hd.TongTien = total;
                    _danhSachHD.Add(hd);
                }
            }
        }

        public void TaoHoaDon(HoaDon hd)
        {
            // Tính tổng tiền hóa đơn
            decimal total = 0;
            foreach (var ct in hd.ChiTiet)
            {
                total += ct.ThanhTien;
                
                // Cập nhật trạng thái sản phẩm sau khi bán thành công
                if(ct.SanPham != null)
                {
                    _productDAO.UpdateProductStatus(ct.SanPham.Id, "Đã bán");
                }
            }
            hd.TongTien = total;
            
            _danhSachHD.Add(hd);
        }

        public List<HoaDon> GetAll()
        {
            return _danhSachHD;
        }
    }
}
