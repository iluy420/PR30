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
    public partial class WorkingWithUserIncomeAndExpense : Page
    {
        private PR29DatabaseEntities _context = new PR29DatabaseEntities();

        private CategoriEnum _CategoriType { get; set; }
        private bool _CategoriTypeIsValid { get; set; }
        private string _Categori { get; set; }
        private string _User { get; set; }
        private DateTime _dateStart { get; set; }
        private DateTime _dateEnd { get; set; }
        private RoleEnum roleUser { get; set; }

        public WorkingWithUserIncomeAndExpense()
        {
            InitializeComponent();

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            roleUser = mainWindow.Role;

            Title = "Работа с доходоми и расходами";

            foreach (CategoriEnum categorType in Enum.GetValues(typeof(CategoriEnum)))
            {
                ComboBox_CategoriType.Items.Add(categorType);
            }
            ComboBox_CategoriType.SelectedValue = CategoriEnum.Income;

            var users = _context.User.ToList();
            foreach (var user in users)
            {
                ComboBox_User.Items.Add(user.Login);
            }
            UpdateTable();

        }

        private void ButtonDell_Click(object sender, RoutedEventArgs e)
        {
            if(roleUser == RoleEnum.Admin)
            {
                switch (_CategoriType)
                {
                    case CategoriEnum.Income:
                        var UserIncomeRemoving = WorkingWithUserIncomeAndExpenseDataGrid.SelectedItems.Cast<UserIncome>().ToList();

                        if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {UserIncomeRemoving.Count()} элементов?", "Внимание",
                                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) ;
                        try
                        {
                            PR29DatabaseEntities.GetContext().UserIncome.RemoveRange(UserIncomeRemoving);
                            PR29DatabaseEntities.GetContext().SaveChanges();
                            MessageBox.Show("Данные успешно удалены!");

                            UpdateTable();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                        break;
                    case CategoriEnum.Expense:
                        var UserExpenseRemoving = WorkingWithUserIncomeAndExpenseDataGrid.SelectedItems.Cast<UserExpense>().ToList();

                        if (MessageBox.Show($"Вы точно хотите удалить записи в количестве {UserExpenseRemoving.Count()} элементов?", "Внимание",
                                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) ;
                        try
                        {
                            PR29DatabaseEntities.GetContext().UserExpense.RemoveRange(UserExpenseRemoving);
                            PR29DatabaseEntities.GetContext().SaveChanges();
                            MessageBox.Show("Данные успешно удалены!");

                            UpdateTable();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                        break;
                }
            }
            else
            {
                MessageBox.Show("Не достаточно прав!");
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (roleUser == RoleEnum.Admin)
            {
                switch (ComboBox_CategoriType.SelectedValue)
                {
                    case CategoriEnum.Income:
                        NavigationService?.Navigate(new AddAndEditUserIncomeAndExpense(((sender as Button).DataContext as UserIncome)));
                        break;

                    case CategoriEnum.Expense:
                        NavigationService?.Navigate(new AddAndEditUserIncomeAndExpense(((sender as Button).DataContext as UserExpense)));
                        break;
                }
            }
            else
            {
                MessageBox.Show("Не достаточно прав!");
            }
        }

        private void ComboBox_CategoriType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox_CategoriType.SelectedValue)
            {
                case CategoriEnum.Income:
                    _CategoriType = CategoriEnum.Income;
                    _CategoriTypeIsValid = true;
                    ComboBox_Categori.Items.Clear();
                    var categorsIncome = _context.IncomeCategorie.ToList();
                    foreach (var categor in categorsIncome)
                    {
                        ComboBox_Categori.Items.Add(categor.Name);
                    }
                    break;

                case CategoriEnum.Expense:
                    _CategoriType = CategoriEnum.Expense;
                    _CategoriTypeIsValid = true;
                    ComboBox_Categori.Items.Clear();
                    var categorsExpense = _context.ExpenseCategory.ToList();
                    foreach (var categor in categorsExpense)
                    {
                        ComboBox_Categori.Items.Add(categor.Name);
                    }
                    break;

                default:
                    _CategoriTypeIsValid = false;
                    break;
            }

            UpdateTable();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            ComboBox_Categori.Items.Clear();
            ComboBox_CategoriType.SelectedValue = null;
            ComboBox_User.SelectedValue = null;
            Calendarx_StartDate.SelectedDate = null;
            Calendarx_EndtDate.SelectedDate = null;
            UpdateTable();
        }

        private void UpdateTable()
        {
            if (_CategoriTypeIsValid)
            {
                switch (_CategoriType)
                {
                    case CategoriEnum.Income:

                        Binding bindingIncome = new Binding();
                        bindingIncome.Path = new PropertyPath("IncomeCategorie.Name");
                        CategoriColum.Binding = bindingIncome;

                        var currentIncome = PR29DatabaseEntities.GetContext().UserIncome.ToList();

                        if (_Categori != null)
                        {
                            currentIncome = currentIncome.Where(x => x.IncomeCategorie.Name == _Categori).ToList();
                        }

                        if (_User != null)
                        {
                            currentIncome = currentIncome.Where(x => x.User.Login == _User).ToList();
                        }

                        DateTime dateTimeIncome = new DateTime();
                        if (_dateStart != dateTimeIncome)
                        {
                            currentIncome = currentIncome.Where(x => x.Data >= _dateStart).ToList();
                        }

                        if (_dateEnd != dateTimeIncome)
                        {
                            currentIncome = currentIncome.Where(x => x.Data <= _dateEnd).ToList();
                        }

                        WorkingWithUserIncomeAndExpenseDataGrid.ItemsSource = currentIncome.ToList(); ;
                        break;
                    case CategoriEnum.Expense:

                        Binding bindingExpense = new Binding();
                        bindingExpense.Path = new PropertyPath("ExpenseCategory.Name");
                        CategoriColum.Binding = bindingExpense;

                        var currentExpense = PR29DatabaseEntities.GetContext().UserExpense.ToList();

                        if (_Categori != null)
                        {
                            currentExpense = currentExpense.Where(x => x.ExpenseCategory.Name == _Categori).ToList();
                        }

                        if (_User != null)
                        {
                            currentExpense = currentExpense.Where(x => x.User.Login == _User).ToList();
                        }

                        DateTime dateTimeExpense = new DateTime();
                        if (_dateStart != dateTimeExpense)
                        {
                            currentExpense = currentExpense.Where(x => x.Data >= _dateStart).ToList();
                        }

                        if (_dateEnd != dateTimeExpense)
                        {
                            currentExpense = currentExpense.Where(x => x.Data <= _dateEnd).ToList();
                        }

                        WorkingWithUserIncomeAndExpenseDataGrid.ItemsSource = currentExpense.ToList();
                        break;
                }
            }
            else
            {
                WorkingWithUserIncomeAndExpenseDataGrid.ItemsSource = null;
            }
        }
            
        private void ComboBox_User_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox_User.SelectedValue != null)
                _User = ComboBox_User.SelectedValue.ToString();
            else
                _User = null;
            UpdateTable();
        }

        private void ComboBox_Categori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox_Categori.SelectedValue != null)
                _Categori = ComboBox_Categori.SelectedValue.ToString();
            else
                _Categori = null;
            UpdateTable();
        }

        private void Calendarx_EndtDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Calendarx_EndtDate.SelectedDate != null)
                _dateEnd = (DateTime)Calendarx_EndtDate.SelectedDate;
            else
                _dateEnd = new DateTime();
            UpdateTable();
        }

        private void Calendarx_StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Calendarx_StartDate.SelectedDate != null)
                _dateStart = (DateTime)Calendarx_StartDate.SelectedDate;
            else
                _dateStart = new DateTime();
            UpdateTable();
        }

        private void AddExpence_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddAndEditUserIncomeAndExpense((UserExpense)null));
        }

        private void AddIncome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddAndEditUserIncomeAndExpense((UserIncome)null));
        }
    }
}
