using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PR30
{
    public partial class WorkingWithUsersPage : Page
    {
        public WorkingWithUsersPage()
        {
            InitializeComponent();
            WorkingWithUsersDataGrid.ItemsSource = PR29DatabaseEntities.GetContext().User.ToList();
            Title = "Работа с пользователями";
            PageName.Text = Title;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegistrationPage(null));
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegistrationPage(((sender as Button).DataContext as User)));
        }

        private void ButtonDell_Click(object sender, RoutedEventArgs e)
        {
            var UsersRemoving = WorkingWithUsersDataGrid.SelectedItems.Cast<User>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {UsersRemoving.Count()} элементов?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes);
            try
            {
                PR29DatabaseEntities.GetContext().User.RemoveRange(UsersRemoving);
                PR29DatabaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно удалены!");

                WorkingWithUsersDataGrid.ItemsSource = PR29DatabaseEntities.GetContext().User.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
