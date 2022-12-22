using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PR30
{
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {

            if(Auth(UserLogin.Text, UserPassword.Password))
            {
                NavigationService?.Navigate(new WorkingWithUserIncomeAndExpense());
            }
            
        }

        public bool Auth(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return false;
            }
            try
            {
                using (var db = new PR29DatabaseEntities())
                {
                    password = GetHashSHA.GetHash(password);
                    var user = db.User.AsNoTracking().FirstOrDefault(u => u.Login == login
                                && u.Password == password);
                    if (user == null)
                    {
                        MessageBox.Show("Пользователь с такими данными не найден!");
                        return false;
                    }

                    MessageBox.Show("Пользователь успешно найден!");

                    if (user.Role == 1)// если обычный мользователь
                    {
                        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                        mainWindow.WorkingWithUsers.Visibility = Visibility.Collapsed;
                        mainWindow.WorkingWithExpenseCategories.Visibility = Visibility.Collapsed;
                        mainWindow.WorkingWithIncomeCategories.Visibility = Visibility.Collapsed;
                        mainWindow.Role = RoleEnum.User;
                    }
                    else
                    {
                        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                        mainWindow.WorkingWithUsers.Visibility = Visibility.Visible;
                        mainWindow.WorkingWithExpenseCategories.Visibility = Visibility.Visible;
                        mainWindow.WorkingWithIncomeCategories.Visibility = Visibility.Visible;
                        mainWindow.Role = RoleEnum.Admin;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Проверьте подклчение к интернету!");
                return false;
            }
            
            return true;
        }
    }
}
