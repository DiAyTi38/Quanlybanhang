using System;
using System.Collections.Generic;
using Quanlybanhang.Models;

namespace Quanlybanhang.DAO
{
    public class HoaDonDAO
    {
        private static List<HoaDon> _danhSachHD = new List<HoaDon>();
        private readonly ProductDAO _productDAO = new ProductDAO();

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
