using System.Collections.ObjectModel;
using System.Windows.Input;
using Quanlybanhang.DAO;
using Quanlybanhang.Models;

namespace Quanlybanhang.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly ProductDAO _dao = new ProductDAO();

        public ObservableCollection<Product> Products { get; set; }

        private Product _newProduct = new Product();
        public Product NewProduct
        {
            get => _newProduct;
            set { _newProduct = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand { get; }

        public ProductViewModel()
        {
            Products = new ObservableCollection<Product>(_dao.GetAllProducts());
            AddCommand = new RelayCommand(AddProduct, CanAddProduct);
        }

        private bool CanAddProduct()
        {
            return !string.IsNullOrWhiteSpace(NewProduct?.Id) && !string.IsNullOrWhiteSpace(NewProduct?.Name);
        }

        private void AddProduct()
        {
            var prod = new Product
            {
                Id = NewProduct.Id?.Trim() ?? string.Empty,
                Name = NewProduct.Name,
                Price = NewProduct.Price,
                IsNew = NewProduct.IsNew,
                Description = NewProduct.Description,
                Status = NewProduct.Status
            };

            var inserted = _dao.AddProduct(prod);

            // Add to observable collection so UI updates immediately
            Products.Insert(0, prod);

            // Reset input
            NewProduct = new Product();
            // Force CanExecute update
            CommandManager.InvalidateRequerySuggested();
        }
        private void DeleteProduct()
        {
            if (SelectedProduct == null)
                return;

            // Gọi DAO xóa trong database
            var deleted = _dao.DeleteProduct(SelectedProduct.Id);

            if (deleted)
            {
                // Xóa khỏi ObservableCollection để update UI
                Products.Remove(SelectedProduct);
            }

            // Reset chọn
            SelectedProduct = null;

            // Update trạng thái nút (CanExecute)
            CommandManager.InvalidateRequerySuggested();
        }
    }
}