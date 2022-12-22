using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    public partial class MainWindow : Window
    {
        public RoleEnum Role { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitAccount_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new LoginPage());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(MessageBox.Show("Вы уверены?", "Вызод", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (!(e.Content is Page page)) return;
            Title = $"PR30 - {page.Title}";

            if (page is LoginPage) MainMenu.Visibility = Visibility.Hidden;
            else MainMenu.Visibility = Visibility.Visible;

            // определяем путь к файлу ресурсов
            var uri = new Uri("Dictionary.xaml", UriKind.Relative);
            // загружаем словарь ресурсов
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            // очищаем коллекцию ресурсов приложения
            Application.Current.Resources.Clear();
            // добавляем загруженный словарь ресурсов
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private void WorkingWithUsers_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new WorkingWithUsersPage());
        }

        private void WorkingWithIncomeCategories_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new WorkingWithIncomeCategoriesPage());
        }

        private void WorkingWithExpenseCategories_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new WorkingWithExpenseCategoriesPage());
        }

        private void ExcelReports_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new ExcelReports());
        }

        private void WordReports_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new WordReports());
        }

        private void WorkingWithIncomeAndExpenses_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new WorkingWithUserIncomeAndExpense());
        }
    }
}
