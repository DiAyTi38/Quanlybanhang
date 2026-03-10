using Quanlybanhang.ViewModels;

namespace Quanlybanhang.Models
{
    public class KhachHang : BaseViewModel
    {
        private string _maKH = string.Empty;
        public string MaKH
        {
            get => _maKH;
            set { _maKH = value; OnPropertyChanged(); }
        }

        private string _tenKH = string.Empty;
        public string TenKH
        {
            get => _tenKH;
            set { _tenKH = value; OnPropertyChanged(); }
        }

        private string _soDienThoai = string.Empty;
        public string SoDienThoai
        {
            get => _soDienThoai;
            set { _soDienThoai = value; OnPropertyChanged(); }
        }
    }
}
