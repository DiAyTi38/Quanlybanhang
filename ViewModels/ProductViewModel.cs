using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
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

        // ✅ FIX LỖI SelectedProduct
        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public ProductViewModel()
        {
            Products = new ObservableCollection<Product>(_dao.GetAllProducts());

            AddCommand = new RelayCommand(AddProduct, CanAddProduct);
            DeleteCommand = new RelayCommand(DeleteProduct, () => SelectedProduct != null);
        }

        private bool CanAddProduct()
        {
            return !string.IsNullOrWhiteSpace(NewProduct?.Id) &&
                   !string.IsNullOrWhiteSpace(NewProduct?.Name);
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

            if (inserted)
                Products.Insert(0, prod);

            NewProduct = new Product();
            CommandManager.InvalidateRequerySuggested();
        }

        private void DeleteProduct()
        {
            if (SelectedProduct == null)
                return;

            var deleted = _dao.DeleteProduct(SelectedProduct.Id);

            if (deleted)
            {
                Products.Remove(SelectedProduct);
            }

            SelectedProduct = null;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}