using System.Windows;
using System.Windows.Controls;
using Quanlybanhang.ViewModels;

namespace Quanlybanhang.Views
{
    public partial class KhachHangView : UserControl
    {
        private KhachHangViewModel vm;

        public KhachHangView()
        {
            InitializeComponent();

            // ✅ Khởi tạo an toàn
            vm = new KhachHangViewModel();

            // ✅ Tránh null DataContext
            this.DataContext = vm;
        }

        private void Them_Click(object sender, RoutedEventArgs e)
        {
            if (vm == null) return; // ✅ chống null
            vm.Them();
        }

        private void Sua_Click(object sender, RoutedEventArgs e)
        {
            if (vm == null) return; // ✅ chống null
            vm.Sua();
        }

        private void Xoa_Click(object sender, RoutedEventArgs e)
        {
            if (vm == null) return; // ✅ chống null
            vm.Xoa();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var wnd = Window.GetWindow(this);

            // ✅ check null trước khi dùng
            if (wnd != null)
            {
                wnd.Close();
            }
            else
            {
                MessageBox.Show("Không tìm thấy cửa sổ cha!");
            }
        }
    }
}