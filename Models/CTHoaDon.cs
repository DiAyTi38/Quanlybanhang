using Quanlybanhang.ViewModels;

namespace Quanlybanhang.Models
{
    public class CTHoaDon : BaseViewModel
    {
        private string _maHD = string.Empty;
        public string MaHD
        {
            get => _maHD;
            set { _maHD = value; OnPropertyChanged(); }
        }

        private Product _sanPham;
        public Product SanPham
        {
            get => _sanPham;
            set 
            { 
                _sanPham = value; 
                OnPropertyChanged(); 
                if (_sanPham != null) 
                {
                    DonGia = _sanPham.Price;
                }
            }
        }

        private int _soLuong = 1;
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

        private decimal _donGia;
        public decimal DonGia
        {
            get => _donGia;
            set 
            { 
                _donGia = value; 
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
            ThanhTien = SoLuong * DonGia;
        }
    }
}
