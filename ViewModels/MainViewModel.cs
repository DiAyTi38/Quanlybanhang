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
        private ObservableCollection<Product> _allProducts;

        // Danh sách sản phẩm đang hiển thị (phục vụ tìm kiếm)
        private ObservableCollection<Product> _products;
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
            // Khởi tạo danh sách mẫu
            _allProducts = new ObservableCollection<Product>
            {
                new Product { Id = "SH01", Name = "Áo khoác da Vintage", Price = 350000, IsNew = false },
                new Product { Id = "SH02", Name = "Giày thể thao Nike Air (Like New)", Price = 800000, IsNew = false },
                new Product { Id = "NEW01", Name = "Áo thun Local Brand (Nguyên tag)", Price = 250000, IsNew = true }
            };

            _products = new ObservableCollection<Product>(_allProducts);

            // Khởi tạo Commands
            AddCommand = new RelayCommand(AddProduct, CanAddProduct);
            DeleteCommand = new RelayCommand<Product>(DeleteProduct);
        }

        private bool CanAddProduct()
        {
            // Chỉ cho phép bấm nút Thêm khi Mã và Tên không bị trống
            return !string.IsNullOrWhiteSpace(NewId) && !string.IsNullOrWhiteSpace(NewName);
        }

        private void AddProduct()
        {
            // Tạo sản phẩm mới từ dữ liệu người dùng nhập
            var newProd = new Product
            {
                Id = NewId,
                Name = NewName,
                Price = NewPrice,
                IsNew = NewIsNew
            };

            // Thêm vào danh sách
            _allProducts.Add(newProd);
            SearchProducts(); // Cập nhật lại list hiển thị

            // Reset form
            NewId = string.Empty;
            NewName = string.Empty;
            NewPrice = 0;
            NewIsNew = false;
        }

        private void DeleteProduct(Product product)
        {
            if (product != null)
            {
                _allProducts.Remove(product);
                SearchProducts(); // Cập nhật lại list hiển thị
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
