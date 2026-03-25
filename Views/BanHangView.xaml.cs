using System.Windows.Controls;

namespace Quanlybanhang.Views
{
    public partial class BanHangView : UserControl
    {
        public BanHangView()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var window = System.Windows.Window.GetWindow(this);
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
