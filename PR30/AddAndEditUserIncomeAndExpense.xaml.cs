using Microsoft.Office.Interop.Word;
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
    public partial class AddAndEditUserIncomeAndExpense : System.Windows.Controls.Page
    {
        private UserExpense _selectedUserExpense = new UserExpense();
        private UserIncome _selectedUserIncome = new UserIncome();
        private int type;
        private PR29DatabaseEntities _context = new PR29DatabaseEntities();

        public AddAndEditUserIncomeAndExpense(UserExpense selectedUserExpense)
        {
            InitializeComponent();

            if (selectedUserExpense != null)
            {
                _selectedUserExpense = selectedUserExpense;

                UserLogin.SelectedItem = _selectedUserExpense.User.Login;
                Category.SelectedItem = _selectedUserExpense.ExpenseCategory.Name;
                Calendarx_Date.SelectedDate = _selectedUserExpense.Data;
                TextBox_Amount.Text = _selectedUserExpense.Amount.ToString();
                TextBox_Description.Text = _selectedUserExpense.Description;

                Title = "Редактирование расхода";
                NamePage.Text = Title;
            }
            else
            {
                Title = "Добавление расхода";
                NamePage.Text = Title;
            }

            var categorsExpense = _context.ExpenseCategory.ToList();
            foreach (var categor in categorsExpense)
            {
                Category.Items.Add(categor.Name);
            }

            var users = _context.User.ToList();
            foreach (var user in users)
            {
                UserLogin.Items.Add(user.Login);
            }

            DataContext = _selectedUserExpense;
            type = 1;
        }

        public AddAndEditUserIncomeAndExpense(UserIncome selectedUserIncom)
        {
            InitializeComponent();
            if (selectedUserIncom != null)
            {
                _selectedUserIncome = selectedUserIncom;

                UserLogin.SelectedItem = _selectedUserIncome.User.Login;
                Category.SelectedItem = _selectedUserIncome.IncomeCategorie.Name;
                Calendarx_Date.SelectedDate = _selectedUserIncome.Data;
                TextBox_Amount.Text = _selectedUserIncome.Amount.ToString();
                TextBox_Description.Text = _selectedUserIncome.Description;


                Title = "Редактирование дохода";
                NamePage.Text = Title;
            }
            else
            {
                Title = "Добавление дохода";
                NamePage.Text = Title;
            }

            var categorsIncome = _context.IncomeCategorie.ToList();
            foreach (var categor in categorsIncome)
            {
                Category.Items.Add(categor.Name);
            }

            var users = _context.User.ToList();
            foreach (var user in users)
            {
                UserLogin.Items.Add(user.Login);
            }

            DataContext = _selectedUserIncome;
            type = 2;
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            switch (type)
            {
                case 1:
                     SevaUserExpense();
                    break;
                case 2:
                    SevaUserIncom();
                    break;
            }
        }

        private void SevaUserIncom()
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(UserLogin.Text))
                errors.AppendLine("Укажите пользователя");
            else
            {
                string nameUser = UserLogin.Text;
                var user = _context.User.Where(x => x.Login == nameUser).First();
                _selectedUserIncome.IdUser = user.IdUser;
            }

            if (string.IsNullOrWhiteSpace(Category.Text))
                errors.AppendLine("Укажите категорию");
            else
            {
                string nameCategory = Category.Text;
                var incomeCategorie = _context.IncomeCategorie.Where(x => x.Name == nameCategory).First();
                _selectedUserIncome.IdCategorieIncome  = incomeCategorie.Id;
            }

            if (Calendarx_Date.SelectedDate == null)
                errors.AppendLine("Укажите дату");
            else
            {
                _selectedUserIncome.Data = (DateTime)Calendarx_Date.SelectedDate;
            }

            if(string.IsNullOrWhiteSpace(TextBox_Amount.Text))
                errors.AppendLine("Укажите сумму");
            else
            {
                try {
                    double var = Convert.ToDouble(TextBox_Amount.Text);
                    _selectedUserIncome.Amount = (decimal)var;
                }
                catch
                {
                    errors.AppendLine("не верная сумма");
                }
            }

            if (string.IsNullOrWhiteSpace(TextBox_Description.Text))
                errors.AppendLine("Укажите описание");
            else _selectedUserIncome.Description = TextBox_Description.Text;


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_selectedUserIncome.Id == 0)
                PR29DatabaseEntities.GetContext().UserIncome.Add(_selectedUserIncome);
            try
            {
                PR29DatabaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                NavigationService?.Navigate(new WorkingWithUserIncomeAndExpense());
            }
            catch
            {
                MessageBox.Show("Проверьте подключение к интернету!");
            }
        }

        private void SevaUserExpense()
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(UserLogin.Text))
                errors.AppendLine("Укажите пользователя");
            else
            {
                string nameUser = UserLogin.Text;
                var user = _context.User.Where(x => x.Login == nameUser).First();
                _selectedUserExpense.IdUser = user.IdUser;
            }

            if (string.IsNullOrWhiteSpace(Category.Text))
                errors.AppendLine("Укажите категорию");
            else
            {
                string nameCategory = Category.Text;
                var expenseCategory = _context.ExpenseCategory.Where(x => x.Name == nameCategory).First();
                _selectedUserExpense.IdCategorieExpense = expenseCategory.Id;
            }

            if (Calendarx_Date.SelectedDate == null)
                errors.AppendLine("Укажите дату");
            else
            {
                _selectedUserExpense.Data = (DateTime)Calendarx_Date.SelectedDate;
            }

            if (string.IsNullOrWhiteSpace(TextBox_Amount.Text))
                errors.AppendLine("Укажите сумму");
            else
            {
                try
                {
                    double var = Convert.ToDouble(TextBox_Amount.Text);
                    _selectedUserExpense.Amount = (decimal)var;
                }
                catch
                {
                    errors.AppendLine("не верная сумма");
                }
            }

            if (string.IsNullOrWhiteSpace(TextBox_Description.Text))
                errors.AppendLine("Укажите описание");
            else _selectedUserExpense.Description = TextBox_Description.Text;


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_selectedUserExpense.Id == 0)
                PR29DatabaseEntities.GetContext().UserExpense.Add(_selectedUserExpense);
            try
            {
                PR29DatabaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные успешно сохранены!");
                NavigationService?.Navigate(new WorkingWithUserIncomeAndExpense());
            }
            catch
            {
                MessageBox.Show("Проверьте подключение к интернету!");
            }
        }
    }
}
