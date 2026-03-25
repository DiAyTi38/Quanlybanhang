using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Quanlybanhang.DAO;
using Quanlybanhang.Models;

namespace Quanlybanhang.ViewModels
{
    public class BanHangViewModel : BaseViewModel
    {
        private readonly ProductDAO _productDAO = new ProductDAO();
        private readonly KhachHangDAO _khDao = new KhachHangDAO();
        private readonly HoaDonDAO _hdDao = new HoaDonDAO();

        public ObservableCollection<KhachHang> Customers { get; set; }
        
        private ObservableCollection<Product> _availableProducts;
        public ObservableCollection<Product> AvailableProducts
        {
            get => _availableProducts;
            set { _availableProducts = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CTHoaDon> Cart { get; set; }

        private KhachHang _selectedCustomer;
        public KhachHang SelectedCustomer
        {
            get => _selectedCustomer;
            set 
            { 
                _selectedCustomer = value; 
                OnPropertyChanged(); 
                CommandManager.InvalidateRequerySuggested(); 
            }
        }

        private decimal _tongTien;
        public decimal TongTien
        {
            get => _tongTien;
            private set { _tongTien = value; OnPropertyChanged(); }
        }

        public ICommand AddToCartCommand { get; }
        public ICommand RemoveFromCartCommand { get; }
        public ICommand CheckoutCommand { get; }

        public BanHangViewModel()
        {
            Customers = new ObservableCollection<KhachHang>(_khDao.GetAll());
            AvailableProducts = new ObservableCollection<Product>(_productDAO.GetAvailableProducts());
            Cart = new ObservableCollection<CTHoaDon>();

            AddToCartCommand = new RelayCommand<Product>(AddToCart);
            RemoveFromCartCommand = new RelayCommand<CTHoaDon>(RemoveFromCart);
            CheckoutCommand = new RelayCommand(Checkout, CanCheckout);

            Cart.CollectionChanged += (s, e) => 
            {
                if (e.NewItems != null)
                {
                    foreach (CTHoaDon item in e.NewItems)
                        item.PropertyChanged += Item_PropertyChanged;
                }
                if (e.OldItems != null)
                {
                    foreach (CTHoaDon item in e.OldItems)
                        item.PropertyChanged -= Item_PropertyChanged;
                }
                CalculateTotal();
                CommandManager.InvalidateRequerySuggested();
            };
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ThanhTien" || e.PropertyName == "SoLuong")
            {
                CalculateTotal();
            }
        }

        private void AddToCart(Product product)
        {
            if (product == null) return;

            var existing = Cart.FirstOrDefault(c => c.SanPham.Id == product.Id);
            if (existing != null)
            {
                existing.SoLuong++;
            }
            else
            {
                Cart.Add(new CTHoaDon
                {
                    SanPham = product,
                    SoLuong = 1,
                    DonGia = product.Price
                });
            }
        }

        private void RemoveFromCart(CTHoaDon ct)
        {
            if (ct == null) return;
            Cart.Remove(ct);
        }

        private void CalculateTotal()
        {
            TongTien = Cart.Sum(c => c.ThanhTien);
        }

        private bool CanCheckout()
        {
            return SelectedCustomer != null && Cart.Count > 0;
        }

        private void Checkout()
        {
            try
            {
                var newMaHd = "HD" + DateTime.Now.ToString("yyMMddHHmmss");
                var hd = new HoaDon
                {
                    MaHD = newMaHd,
                    KhachHang = SelectedCustomer,
                    NgayLap = DateTime.Now
                };

                foreach (var item in Cart)
                {
                    item.MaHD = newMaHd;
                    hd.ChiTiet.Add(item);
                }

                // Lưu hoá đơn (bên trong sẽ UpdateProductStatus thành "Đã bán")
                _hdDao.TaoHoaDon(hd);
                
                // Cập nhật doanh thu ngày vào CSDL
                var dtDao = new DoanhThuDAO();
                dtDao.AddDoanhThu(DateTime.Now, TongTien);
                
                MessageBox.Show($"Xuất hoá đơn thành công!\nThu: {TongTien:N0} VNĐ", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // Reload lại kho hàng
                Cart.Clear();
                SelectedCustomer = null;
                TongTien = 0;
                AvailableProducts = new ObservableCollection<Product>(_productDAO.GetAvailableProducts());
                CommandManager.InvalidateRequerySuggested();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất hoá đơn: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
