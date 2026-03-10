using System;
using System.Collections.ObjectModel;
using Quanlybanhang.ViewModels;

namespace Quanlybanhang.Models
{
    public class PhieuNhap : BaseViewModel
    {
        private string _maPN = string.Empty;
        public string MaPN
        {
            get => _maPN;
            set { _maPN = value; OnPropertyChanged(); }
        }

        private DateTime _ngayNhap = DateTime.Now;
        public DateTime NgayNhap
        {
            get => _ngayNhap;
            set { _ngayNhap = value; OnPropertyChanged(); }
        }

        private decimal _tongTien;
        public decimal TongTien
        {
            get => _tongTien;
            set { _tongTien = value; OnPropertyChanged(); }
        }

        // Danh sách chi tiết phiếu nhập
        public ObservableCollection<CTPhieuNhap> ChiTiet { get; set; } = new ObservableCollection<CTPhieuNhap>();
    }
}
