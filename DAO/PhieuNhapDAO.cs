using System;
using System.Collections.Generic;
using Quanlybanhang.Models;

namespace Quanlybanhang.DAO
{
    public class PhieuNhapDAO
    {
        // Mock Database cho Phiếu nhập
        private static List<PhieuNhap> _danhSachPN = new List<PhieuNhap>();

        public void TaoPhieuNhap(PhieuNhap pn)
        {
            _danhSachPN.Add(pn);
            
            // Tính tổng tiền dựa trên các chi tiết phiếu nhập
            decimal total = 0;
            foreach (var ct in pn.ChiTiet)
            {
                total += ct.ThanhTien;
            }
            pn.TongTien = total;
        }

        public void ThemChiTiet(PhieuNhap pn, CTPhieuNhap ct)
        {
            pn.ChiTiet.Add(ct);
            // Có thể gọi update DB ở đây (nếu có DB thực)
        }

        public List<PhieuNhap> GetAll()
        {
            return _danhSachPN;
        }
    }
}
