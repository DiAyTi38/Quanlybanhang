using System;
using System.Collections.ObjectModel;
using Quanlybanhang.ViewModels;

namespace Quanlybanhang.Models
{
    public class HoaDon : BaseViewModel
    {
        private string _maHD = string.Empty;
        public string MaHD
        {
            get => _maHD;
            set { _maHD = value; OnPropertyChanged(); }
        }

        private KhachHang _khachHang;
        public KhachHang KhachHang
        {
            get => _khachHang;
            set { _khachHang = value; OnPropertyChanged(); }
        }

        private DateTime _ngayLap = DateTime.Now;
        public DateTime NgayLap
        {
            get => _ngayLap;
            set { _ngayLap = value; OnPropertyChanged(); }
        }

        private decimal _tongTien;
        public decimal TongTien
        {
            get => _tongTien;
            set { _tongTien = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CTHoaDon> ChiTiet { get; set; } = new ObservableCollection<CTHoaDon>();
    }
}
