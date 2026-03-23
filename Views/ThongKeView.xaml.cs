using System.Windows;
using System.Windows.Controls;
using Quanlybanhang.ViewModels;

namespace Quanlybanhang.Views
{
    public partial class ThongKeView : UserControl
    {
        private ThongKeViewModel vm => (ThongKeViewModel)DataContext;

        public ThongKeView()
        {
            InitializeComponent();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            // recreate VM data by creating a new instance and reassigning DataContext
            DataContext = new ThongKeViewModel();
        }
    }
}
