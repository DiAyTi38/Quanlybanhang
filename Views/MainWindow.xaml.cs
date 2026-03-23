using System.Windows;

namespace Quanlybanhang.Views
{
    public partial class MainWindow : Window
    {
        private Window? _khachHangWindow;
        private Window? _thongKeWindow;

        private string role = ""; // ✅ Đưa vào trong class

        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetRole(string r)
        {
            role = r;

            if (role == "admin")
            {
                MessageBox.Show("Bạn là ADMIN");
            }
            else
            {
                MessageBox.Show("Bạn là NHÂN VIÊN");
            }
        }

        private void ShowThongKe_Click(object sender, RoutedEventArgs e)
        {
            if (_khachHangWindow != null)
            {
                _khachHangWindow.Close();
                _khachHangWindow = null;
            }

            if (_thongKeWindow == null)
            {
                _thongKeWindow = new Window
                {
                    Title = "Thống kê doanh thu",
                    Content = new ThongKeView(),
                    Owner = this,
                    Width = 600,
                    Height = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                _thongKeWindow.Closed += (s, ev) =>
                {
                    _thongKeWindow = null;
                    ProductsGrid.Visibility = Visibility.Visible;
                };

                ProductsGrid.Visibility = Visibility.Collapsed;
                _thongKeWindow.Show();
            }
            else
            {
                _thongKeWindow.Activate();
            }
        }

        private void ShowKhachHang_Click(object sender, RoutedEventArgs e)
        {
            if (_thongKeWindow != null)
            {
                _thongKeWindow.Close();
                _thongKeWindow = null;
            }

            if (_khachHangWindow == null)
            {
                _khachHangWindow = new Window
                {
                    Title = "Quản lý Khách hàng",
                    Content = new KhachHangView(),
                    Owner = this,
                    Width = 800,
                    Height = 600,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                _khachHangWindow.Closed += (s, ev) =>
                {
                    _khachHangWindow = null;
                    ProductsGrid.Visibility = Visibility.Visible;
                };

                ProductsGrid.Visibility = Visibility.Collapsed;
                _khachHangWindow.Show();
            }
            else
            {
                _khachHangWindow.Activate();
            }
        }

        private void ShowProducts_Click(object sender, RoutedEventArgs e)
        {
            if (_khachHangWindow != null)
            {
                _khachHangWindow.Close();
                _khachHangWindow = null;
            }

            ProductsGrid.Visibility = Visibility.Visible;
        }
    }
}