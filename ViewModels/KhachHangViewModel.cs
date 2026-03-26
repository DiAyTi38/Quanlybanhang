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

        private KhachHang _khachHangDangChon;
        public KhachHang KhachHangDangChon
        {
            get => _khachHangDangChon;
            set
            {
                _khachHangDangChon = value ?? new KhachHang(); // ✅ chống null
                OnPropertyChanged();
            }
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
            try
            {
                var data = _dao.GetAll() ?? new System.Collections.Generic.List<KhachHang>();

                _allItems = new ObservableCollection<KhachHang>(data);
                DanhSachKhachHang = new ObservableCollection<KhachHang>(_allItems);

                // ✅ QUAN TRỌNG NHẤT: KHÔNG BAO GIỜ NULL
                KhachHangDangChon = new KhachHang();
            }
            catch
            {
                _allItems = new ObservableCollection<KhachHang>();
                DanhSachKhachHang = new ObservableCollection<KhachHang>();
                KhachHangDangChon = new KhachHang();
            }
        }

        private void ApplyFilter()
        {
            var q = TuKhoa?.Trim();

            DanhSachKhachHang.Clear();

            var source = string.IsNullOrEmpty(q)
                ? _allItems
                : new ObservableCollection<KhachHang>(_allItems.Where(k =>
                    (!string.IsNullOrEmpty(k.MaKH) && k.MaKH.ToLower().Contains(q.ToLower())) ||
                    (!string.IsNullOrEmpty(k.TenKH) && k.TenKH.ToLower().Contains(q.ToLower())) ||
                    (!string.IsNullOrEmpty(k.SoDienThoai) && k.SoDienThoai.ToLower().Contains(q.ToLower()))
                ));

            foreach (var it in source)
                DanhSachKhachHang.Add(it);
        }

        // ================= THÊM =================
        public void Them()
        {
            try
            {
                if (KhachHangDangChon == null) return; // ✅ chống null

                var newKh = new KhachHang
                {
                    MaKH = KhachHangDangChon.MaKH,
                    TenKH = KhachHangDangChon.TenKH,
                    SoDienThoai = KhachHangDangChon.SoDienThoai
                };

                _dao.AddKhachHang(newKh);

                _allItems.Add(newKh);
                ApplyFilter();

                // ✅ reset form (quan trọng)
                KhachHangDangChon = new KhachHang();
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        // ================= SỬA =================
        public void Sua()
        {
            try
            {
                if (KhachHangDangChon == null) return;

                var existing = _allItems.FirstOrDefault(k => k.MaKH == KhachHangDangChon.MaKH);

                if (existing != null)
                {
                    existing.TenKH = KhachHangDangChon.TenKH;
                    existing.SoDienThoai = KhachHangDangChon.SoDienThoai;

                    _dao.UpdateKhachHang(existing);
                    ApplyFilter();
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        // ================= XÓA =================
        public void Xoa()
        {
            try
            {
                if (KhachHangDangChon == null) return;

                var existing = _allItems.FirstOrDefault(k => k.MaKH == KhachHangDangChon.MaKH);

                if (existing != null)
                {
                    bool deleted = _dao.DeleteKhachHang(existing.MaKH);

                    if (deleted)
                    {
                        _allItems.Remove(existing);
                        DanhSachKhachHang.Remove(existing);

                        // ✅ reset tránh null sau khi xóa
                        KhachHangDangChon = new KhachHang();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Không thể xoá dữ liệu từ Database.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}