using System.Windows;
using System.Windows.Controls;
using Quanlybanhang.ViewModels;

namespace Quanlybanhang.Views
{
    public partial class KhachHangView : UserControl
    {
        KhachHangViewModel vm = new KhachHangViewModel();

        public KhachHangView()
        {
            InitializeComponent();
		DataContext = vm;
        }

        private void Them_Click(object sender, RoutedEventArgs e)
        {
            vm.Them();
        }

        private void Sua_Click(object sender, RoutedEventArgs e)
        {
            vm.Sua();
        }

        private void Xoa_Click(object sender, RoutedEventArgs e)
        {
            vm.Xoa();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            // Close parent window if this UserControl is hosted in a Window
            var wnd = Window.GetWindow(this);
            if (wnd != null)
            {
                wnd.Close();
            }
        }
    }
}