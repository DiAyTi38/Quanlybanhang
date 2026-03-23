
using System.Collections.ObjectModel;
using System.Linq;
using Quanlybanhang.DAO;
using Quanlybanhang.Models;

namespace Quanlybanhang.ViewModels
{
    public class KhachHangViewModel : BaseViewModel
    {
        private KhachHangDAO _dao = new KhachHangDAO();

        public ObservableCollection<KhachHang> DanhSachKhachHang { get; set; }

        private KhachHang _khachHangDangChon = new KhachHang();
        public KhachHang KhachHangDangChon
        {
            get => _khachHangDangChon;
            set { _khachHangDangChon = value; OnPropertyChanged(); }
        }

        private string _tuKhoa = string.Empty;
        public string TuKhoa
        {
            get => _tuKhoa;
            set
            {
                _tuKhoa = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        private ObservableCollection<KhachHang> _allItems;

        public KhachHangViewModel()
        {
            _allItems = new ObservableCollection<KhachHang>(_dao.GetAll());
            DanhSachKhachHang = new ObservableCollection<KhachHang>(_allItems);
        }

        private void ApplyFilter()
        {
            var q = TuKhoa?.Trim();
            if (string.IsNullOrEmpty(q))
            {
                // reset
                DanhSachKhachHang.Clear();
                foreach (var it in _allItems)
                    DanhSachKhachHang.Add(it);
                return;
            }

            var filtered = _allItems.Where(k =>
                (!string.IsNullOrEmpty(k.MaKH) && k.MaKH.IndexOf(q, System.StringComparison.OrdinalIgnoreCase) >= 0) ||
                (!string.IsNullOrEmpty(k.TenKH) && k.TenKH.IndexOf(q, System.StringComparison.OrdinalIgnoreCase) >= 0) ||
                (!string.IsNullOrEmpty(k.SoDienThoai) && k.SoDienThoai.IndexOf(q, System.StringComparison.OrdinalIgnoreCase) >= 0)
            ).ToList();

            DanhSachKhachHang.Clear();
            foreach (var it in filtered)
                DanhSachKhachHang.Add(it);
        }

        public void Them()
        {
            var newKh = new KhachHang
            {
                MaKH = KhachHangDangChon.MaKH,
                TenKH = KhachHangDangChon.TenKH,
                SoDienThoai = KhachHangDangChon.SoDienThoai
            };
            _dao.AddKhachHang(newKh);
            _allItems.Add(newKh);
            ApplyFilter();
        }

        public void Sua()
        {
            var existing = _allItems.FirstOrDefault(k => k == KhachHangDangChon || k.MaKH == KhachHangDangChon.MaKH);
            if (existing != null)
            {
                existing.TenKH = KhachHangDangChon.TenKH;
                existing.SoDienThoai = KhachHangDangChon.SoDienThoai;
                _dao.UpdateKhachHang(existing);
                ApplyFilter();
            }
        }

        public void Xoa()
        {
            var existing = _allItems.FirstOrDefault(k => k == KhachHangDangChon || k.MaKH == KhachHangDangChon.MaKH);
            if (existing != null)
            {
                // try delete from DB, only remove from UI if DB deletion succeeded
                bool deleted = _dao.DeleteKhachHang(existing.MaKH);

                if (deleted)
                {
                    _allItems.Remove(existing);
                    DanhSachKhachHang.Remove(existing);
                }
                else
                {
                    // If delete failed, still remove from UI only if you prefer; currently we keep it and you can inspect Debug output.
                    // Optionally: remove anyway to reflect user's action despite DB issue:
                    // _allItems.Remove(existing);
                    // DanhSachKhachHang.Remove(existing);
                }
            }
        }
    }
}