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
    public partial class WorkingWithExpenseCategoriesPage : Page
    {
        public WorkingWithExpenseCategoriesPage()
        {
            InitializeComponent();

            Title = "Работа категориями расходов";
            PageName.Text = Title;

            WorkingWithExpenseCategoriesDataGrid.ItemsSource = PR29DatabaseEntities.GetContext().ExpenseCategory.ToList();
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddAndEditCategory(((sender as Button).DataContext as ExpenseCategory)));
        }

        private void ButtonDell_Click(object sender, RoutedEventArgs e)
        {
            var CategoriesRemoving = WorkingWithExpenseCategoriesDataGrid.SelectedItems.Cast<ExpenseCategory>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {CategoriesRemoving.Count()} элементов?", "Внимание",
                            MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) ;
            try
            {
                PR29DatabaseEntities.GetContext().ExpenseCategory.RemoveRange(CategoriesRemoving);
                PR29DatabaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно удалены!");

                WorkingWithExpenseCategoriesDataGrid.ItemsSource = PR29DatabaseEntities.GetContext().ExpenseCategory.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void AddExpenseCategories_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddAndEditCategory((ExpenseCategory)null));
        }
    }
}
