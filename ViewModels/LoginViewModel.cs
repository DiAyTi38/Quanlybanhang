using System.ComponentModel;
using Quanlybanhang.DAO;

public class LoginViewModel : INotifyPropertyChanged
{
    private string username;
    public string Username
    {
        get { return username; }
        set
        {
            username = value;
            OnPropertyChanged(nameof(Username));
        }
    }

    private TaiKhoan currentUser;
    public TaiKhoan CurrentUser
    {
        get { return currentUser; }
        set
        {
            currentUser = value;
            OnPropertyChanged(nameof(CurrentUser));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public bool DangNhap(string password)
    {
        TaiKhoanDAO dao = new TaiKhoanDAO();

        CurrentUser = dao.DangNhap(Username, password);

        return CurrentUser != null;
    }
}