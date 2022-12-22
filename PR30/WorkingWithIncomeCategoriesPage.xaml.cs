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
    /// <summary>
    /// Логика взаимодействия для WorkingWithIncomeCategoriesPage.xaml
    /// </summary>
    public partial class WorkingWithIncomeCategoriesPage : Page
    {
        public WorkingWithIncomeCategoriesPage()
        {
            InitializeComponent();

            Title = "Работа с категориями дохода";
            PageName.Text = Title;

            WorkingWithIncomeCategoriesDataGrid.ItemsSource = PR29DatabaseEntities.GetContext().IncomeCategorie.ToList();
        }

        private void AddIncomeCategories_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddAndEditCategory((IncomeCategorie)null));
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddAndEditCategory(((sender as Button).DataContext as IncomeCategorie)));
        }

        private void ButtonDell_Click(object sender, RoutedEventArgs e)
        {
            var CategoriesRemoving = WorkingWithIncomeCategoriesDataGrid.SelectedItems.Cast<IncomeCategorie>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {CategoriesRemoving.Count()} элементов?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) ;
            try
            {
                PR29DatabaseEntities.GetContext().IncomeCategorie.RemoveRange(CategoriesRemoving);
                PR29DatabaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно удалены!");

                WorkingWithIncomeCategoriesDataGrid.ItemsSource = PR29DatabaseEntities.GetContext().IncomeCategorie.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
