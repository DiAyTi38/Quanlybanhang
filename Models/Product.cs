using Quanlybanhang.ViewModels;

namespace Quanlybanhang.Models
{
    public class Product : BaseViewModel
    {
        private string _id = string.Empty;
        public string Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }

        private bool _isNew;
        public bool IsNew
        {
            get => _isNew;
            set { _isNew = value; OnPropertyChanged(); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        private string _status = "Còn hàng"; // Trạng thái: Còn hàng, Đã bán
        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        // Tình trạng hiển thị trên UI
        public string Condition => IsNew ? "Mới 100%" : "Đã qua sử dụng (Secondhand)";
    }
}
