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
    public partial class AddAndEditCategory : Page
    {
        public AddAndEditCategory(ExpenseCategory selectedCategor)
        {
            InitializeComponent();
            if (selectedCategor != null)
            {
                _selectedExpenseCategor = selectedCategor;
                CategoryName.Text = _selectedExpenseCategor.Name;

                Title = "Редактирование категории расхода";
                NamePage.Text = Title;
            }
            else
            {
                Title = "Добавление новой категории расхода";
                NamePage.Text = Title;
            }
            DataContext = _selectedExpenseCategor;
            type = 1;
        }

        public AddAndEditCategory(IncomeCategorie selectedCategor)
        {
            InitializeComponent();
            if (selectedCategor != null)
            {
                _selectedIncomeCategor = selectedCategor;
                CategoryName.Text = _selectedIncomeCategor.Name;

                Title = "Редактирование категории дохода";
                NamePage.Text = Title;
            }
            else
            {
                Title = "Добавление новой категории дохода";
                NamePage.Text = Title;
            }
            DataContext = _selectedIncomeCategor;
            type = 2;
        }

        private ExpenseCategory _selectedExpenseCategor = new ExpenseCategory();
        private IncomeCategorie _selectedIncomeCategor = new IncomeCategorie();
        private int type;

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            switch (type)
            {
                case 1:
                    SevaExpenseCategory();
                    break;
                case 2:
                    SevaIncomeCategory();
                    break;
            }
        }
        
        private void SevaExpenseCategory()
        {
            StringBuilder errors = new StringBuilder();
            
            if (string.IsNullOrWhiteSpace(CategoryName.Text))
                errors.AppendLine("Укажите название категории!");
            else _selectedExpenseCategor.Name = CategoryName.Text;

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_selectedExpenseCategor.Id == 0)
                PR29DatabaseEntities.GetContext().ExpenseCategory.Add(_selectedExpenseCategor);
            try
            {
                PR29DatabaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                NavigationService?.Navigate(new WorkingWithExpenseCategoriesPage());
            }
            catch
            {
                MessageBox.Show("Проверьте подключение к интернету!");
            }
        }

        private void SevaIncomeCategory()
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(CategoryName.Text))
                errors.AppendLine("Укажите название категории!");
            else _selectedIncomeCategor.Name = CategoryName.Text;

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_selectedIncomeCategor.Id == 0)
                PR29DatabaseEntities.GetContext().IncomeCategorie.Add(_selectedIncomeCategor);
            try
            {
                PR29DatabaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                NavigationService?.Navigate(new WorkingWithIncomeCategoriesPage());
            }
            catch
            {
                MessageBox.Show("Проверьте подключение к интернету!");
            }
        }
    }
}
