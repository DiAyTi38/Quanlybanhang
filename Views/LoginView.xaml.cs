using System.Windows;
using Quanlybanhang.ViewModels;

namespace Quanlybanhang.Views
{
    public partial class LoginView : Window
    {
        LoginViewModel vm = new LoginViewModel();

        public LoginView()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string pass = txtPassword.Password;

            if (vm.DangNhap(pass))
            {
                MessageBox.Show("Đăng nhập thành công");

                MainWindow main = new MainWindow();

                main.SetRole(vm.CurrentUser.Quyen);

                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai tài khoản hoặc mật khẩu");
            }
        }
    }
}