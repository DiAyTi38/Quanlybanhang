using Quanlybanhang.ViewModels;

namespace Quanlybanhang.Models
{
    public class CTPhieuNhap : BaseViewModel
    {
        private string _maPN = string.Empty;
        public string MaPN
        {
            get => _maPN;
            set { _maPN = value; OnPropertyChanged(); }
        }

        private string _maSP = string.Empty;
        public string MaSP
        {
            get => _maSP;
            set { _maSP = value; OnPropertyChanged(); }
        }

        private int _soLuong;
        public int SoLuong
        {
            get => _soLuong;
            set 
            { 
                _soLuong = value; 
                OnPropertyChanged(); 
                TinhThanhTien();
            }
        }

        private decimal _donGiaNhap;
        public decimal DonGiaNhap
        {
            get => _donGiaNhap;
            set 
            { 
                _donGiaNhap = value; 
                OnPropertyChanged(); 
                TinhThanhTien();
            }
        }

        private decimal _thanhTien;
        public decimal ThanhTien
        {
            get => _thanhTien;
            private set { _thanhTien = value; OnPropertyChanged(); }
        }

        private void TinhThanhTien()
        {
            ThanhTien = SoLuong * DonGiaNhap;
        }
    }
}
