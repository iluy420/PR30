using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PR30
{
    public partial class RegistrationPage : Page
    {
        private User _selectedUser = new User();
        private bool HashPassword = true;
        private bool NewUserName = true;

        public RegistrationPage(User selectedUser)
        {
            InitializeComponent();

            CheckBox_Password.IsChecked = true;

            if (selectedUser != null)
            {
                _selectedUser = selectedUser;
                HashPassword = false;
                UserLogin.Text = _selectedUser.Login;

                Title = "Редактирование пользователя";
                NamePage.Text = Title;
                NewUserName = false;
            }
            else
            {
                CheckBox_Password.Visibility = Visibility.Collapsed;
                LabelCheckBox_Password.Visibility = Visibility.Collapsed;

                Grid.SetRow(LabelComboBox_Role, 4);
                Grid.SetRow(ComboBox_Role, 4);
                Grid.SetRow(Btn_Registration, 5);

                Title = "Регистрация нового пользователя";
                NamePage.Text = Title;
                
            }
            
            DataContext = _selectedUser;

            foreach (RoleEnum role in Enum.GetValues(typeof(RoleEnum)))
            {
                ComboBox_Role.Items.Add(role);
            }
            ComboBox_Role.SelectedValue = RoleEnum.User;
        }

        private void Btn_Registration_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(UserLogin.Text))
                errors.AppendLine("Укажите логин!");
            else if (NewUserName)
            {
                if (LoginIsGood(UserLogin.Text))
                {
                    _selectedUser.Login = UserLogin.Text;
                }
                else
                {
                    errors.AppendLine("Недопустимый логин!");
                }
            }
            else
            {
                _selectedUser.Login = UserLogin.Text;
            }
            

            if (HashPassword || CheckBox_Password.IsChecked == true)
            {
                if (string.IsNullOrWhiteSpace(UserPassword.Password))
                    errors.AppendLine("Укажите пароль!");
                else if (string.IsNullOrWhiteSpace(UserCopuPassword.Password))
                    errors.AppendLine("Укажите повторно пароль!");
                else if (PasswordIsGood(UserPassword.Password, UserCopuPassword.Password))
                {
                    _selectedUser.Password = GetHashSHA.GetHash(UserPassword.Password);
                }
                else
                {
                    errors.AppendLine("Недопустимый пароль!");
                }

            }

            if (string.IsNullOrWhiteSpace(ComboBox_Role.Text))
                errors.AppendLine("Укажите роль!");
            else _selectedUser.Role = (int)Enum.Parse(typeof(RoleEnum), ComboBox_Role.Text);



            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_selectedUser.IdUser == 0)
                PR29DatabaseEntities.GetContext().User.Add(_selectedUser);
            try
            {
                PR29DatabaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                NavigationService?.Navigate(new WorkingWithUsersPage());
            }
            catch
            {
                MessageBox.Show("Проверьте подключение к интернету!");
            }
        }

        public bool LoginIsGood(string loginUser)
        {
            using (var db = new PR29DatabaseEntities())
            {
                try
                {
                    var login = db.User.FirstOrDefault(u => u.Login == loginUser);

                    if (login != null)
                    {
                        MessageBox.Show("Логин занят!");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch
                {
                    MessageBox.Show("Проверьте подключение к интернету!");
                    return false;
                }
            }
        }


        public bool PasswordIsGood(string password, string passwordCopy)
        {
            if (password != passwordCopy)
            {
                MessageBox.Show("Пароли не совпадают!");
                return false;
            }

            if (password.Length >= 6)
            {
                bool letter = false;//буква
                bool en = true; // английская раскладка
                bool symbol = false; // символ
                bool number = false; // цифра

                for (int i = 0; i < password.Length; i++) // перебираем символы
                {
                    if (password[i] >= 'A' && password[i] <= 'Z' || password[i] >= 'a' && password[i] <= 'z') letter = true; // если есть хотябы одна буква
                    if (password[i] >= 'А' && password[i] <= 'Я' || password[i] >= 'а' && password[i] <= 'я') en = false; // если русская раскладка
                    if (password[i] >= '0' && password[i] <= '9') number = true; // если цифры
                    if (password[i] == '_' || password[i] == '-' || password[i] == '!') symbol = true; // если символ
                }

                if (!en)
                {
                    MessageBox.Show("Доступна только английская раскладка"); // выводим сообщение
                    return false;
                }
                else if (!letter)
                {
                    MessageBox.Show("Добавьте хотя бы одну букву"); // выводим сообщение
                    return false;
                }
                else if (!symbol)
                {
                    MessageBox.Show("Добавьте один из следующих символов: _ - !"); // выводим сообщение
                    return false;
                }
                else if (!number)
                {
                    MessageBox.Show("Добавьте хотя бы одну цифру"); // выводим сообщение
                    return false;
                }
            }
            else
            {
                MessageBox.Show("пароль слишком короткий, минимум 6 символов");
                return false;
            }

            return true;
        }

        private void CheckBox_Password_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBox_Password.IsChecked == true)
            {
                UserPassword.Visibility = Visibility.Visible;
                UserCopuPassword.Visibility = Visibility.Visible;
                LabelUserCopuPassword.Visibility = Visibility.Visible;
                LabelUserPassword.Visibility = Visibility.Visible;

                Grid.SetRow(CheckBox_Password, 4);
                Grid.SetRow(LabelCheckBox_Password, 4);
                Grid.SetRow(LabelComboBox_Role, 5);
                Grid.SetRow(ComboBox_Role, 5);
                Grid.SetRow(Btn_Registration, 6);
            }
            else
            {
                UserPassword.Visibility = Visibility.Collapsed;
                UserCopuPassword.Visibility = Visibility.Collapsed;
                LabelUserCopuPassword.Visibility = Visibility.Collapsed;
                LabelUserPassword.Visibility = Visibility.Collapsed;

                Grid.SetRow(CheckBox_Password, 2);
                Grid.SetRow(LabelCheckBox_Password, 2);
                Grid.SetRow(LabelComboBox_Role, 3);
                Grid.SetRow(ComboBox_Role, 3);
                Grid.SetRow(Btn_Registration, 4);

            }
        }
    }
}
