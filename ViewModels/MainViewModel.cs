using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Quanlybanhang.Models;

namespace Quanlybanhang.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // Danh sách sản phẩm gốc
        private ObservableCollection<Product> _allProducts = new ObservableCollection<Product>();

        // Danh sách sản phẩm đang hiển thị (phục vụ tìm kiếm)
        private ObservableCollection<Product> _products = new ObservableCollection<Product>();
        public ObservableCollection<Product> Products
        {
            get => _products;
            set { _products = value; OnPropertyChanged(); }
        }

        // --- Các Property cho việc Thêm mới ---
        private string _newId = string.Empty;
        public string NewId
        {
            get => _newId;
            set { _newId = value; OnPropertyChanged(); }
        }

        private string _newName = string.Empty;
        public string NewName
        {
            get => _newName;
            set { _newName = value; OnPropertyChanged(); }
        }

        private decimal _newPrice;
        public decimal NewPrice
        {
            get => _newPrice;
            set { _newPrice = value; OnPropertyChanged(); }
        }

        private bool _newIsNew; // True = Mới 100%, False = Secondhand
        public bool NewIsNew
        {
            get => _newIsNew;
            set { _newIsNew = value; OnPropertyChanged(); }
        }

        // --- Các Property cho việc Tìm kiếm ---
        private string _searchKeyword = string.Empty;
        public string SearchKeyword
        {
            get => _searchKeyword;
            // Tự động tìm kiếm ngay khi người dùng gõ phím
            set
            {
                _searchKeyword = value;
                OnPropertyChanged();
                SearchProducts();
            }
        }

        // --- Commands ---
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel()
        {
            LoadData();

            // Khởi tạo Commands
            AddCommand = new RelayCommand(AddProduct, CanAddProduct);
            DeleteCommand = new RelayCommand<Product>(DeleteProduct);
        }

        public void LoadData()
        {
            // Try to load available products from database via DAO
            try
            {
                var dao = new Quanlybanhang.DAO.ProductDAO();
                // Load only available products (per project guideline)
                var list = dao.GetAvailableProducts().ToList();
                _allProducts = new ObservableCollection<Product>(list);
            }
            catch
            {
                // Fallback sample data
                _allProducts = new ObservableCollection<Product>
                {
                    new Product { Id = "SH01", Name = "Áo khoác da Vintage", Price = 350000, IsNew = false },
                    new Product { Id = "SH02", Name = "Giày thể thao Nike Air (Like New)", Price = 800000, IsNew = false },
                    new Product { Id = "NEW01", Name = "Áo thun Local Brand (Nguyên tag)", Price = 250000, IsNew = true }
                };
            }

            Products = new ObservableCollection<Product>(_allProducts);
            SearchKeyword = string.Empty;
        }

        private bool CanAddProduct()
        {
            // Chỉ cho phép bấm nút Thêm khi Mã và Tên không bị trống
            return !string.IsNullOrWhiteSpace(NewId) && !string.IsNullOrWhiteSpace(NewName);
        }

        private void AddProduct()
        {
            try
            {
                var newProd = new Product
                {
                    Id = NewId,
                    Name = NewName,
                    Price = NewPrice,
                    IsNew = NewIsNew,
                    Description = "",
                    Status = "Còn hàng"
                };

                var dao = new Quanlybanhang.DAO.ProductDAO();

                bool result = dao.AddProduct(newProd);

                if (!result) return;

                // reload lại từ database (quan trọng)
                LoadData();

                // reset form
                NewId = string.Empty;
                NewName = string.Empty;
                NewPrice = 0;
                NewIsNew = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void DeleteProduct(Product product)
        {
            if (product == null) return;

            var confirm = System.Windows.MessageBox.Show(
                $"Bạn có chắc chắn muốn xoá sản phẩm: {product.Name} (Mã: {product.Id})?",
                "Xác nhận xoá",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (confirm != System.Windows.MessageBoxResult.Yes) return;

            try
            {
                var dao = new Quanlybanhang.DAO.ProductDAO();
                bool deleted = dao.DeleteProduct(product.Id);

                if (deleted)
                {
                    _allProducts.Remove(product);
                    SearchProducts(); // Cập nhật lại list hiển thị
                    System.Windows.MessageBox.Show("Xoá sản phẩm thành công!", "Thành công", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                }
                else
                {
                    System.Windows.MessageBox.Show("Không tìm thấy sản phẩm này trong kho để xoá.", "Thất bại", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    "Lỗi khi xoá sản phẩm.\n\nLưu ý: Bạn không thể xoá một sản phẩm đã có lịch sử gắn với hoá đơn mua hàng. Tốt nhất hãy dùng chức năng Ẩn (nếu có).\n\nChi tiết lỗi CSDL: " + ex.Message,
                    "Không thể xoá",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        private void SearchProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchKeyword))
            {
                // Nếu chưa gõ gì thì hiện tất cả
                Products = new ObservableCollection<Product>(_allProducts);
            }
            else
            {
                // Lọc danh sách theo Tên hoặc Mã
                string keyword = SearchKeyword.ToLower();
                var filteredResult = _allProducts.Where(p =>
                    p.Name.ToLower().Contains(keyword) ||
                    p.Id.ToLower().Contains(keyword)
                );

                Products = new ObservableCollection<Product>(filteredResult);
            }
        }
    }
}