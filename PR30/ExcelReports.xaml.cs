using Microsoft.Office.Interop.Excel;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace PR30
{
    public partial class ExcelReports : System.Windows.Controls.Page
    {
        private PR29DatabaseEntities _context = new PR29DatabaseEntities();

        private DateTime dateStart { get; set; }
        private DateTime dateEnd { get; set; }

        public ExcelReports()
        {
            InitializeComponent();

            Title = "Формирование отчетов Excel";

            foreach (CategoriEnum categor in Enum.GetValues(typeof(CategoriEnum)))
            {
                ComboBox_Categori.Items.Add(categor);
            }
            ComboBox_Categori.SelectedValue = CategoriEnum.Income;
        }

        private void Btn_ExcelReports_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Calendarx_StartDate.Text))
                errors.AppendLine("Дату начала!");

            if (string.IsNullOrWhiteSpace(Calendarx_EndtDate.Text))
                errors.AppendLine("Дату окончания!");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            dateStart = (DateTime)Calendarx_StartDate.SelectedDate;
            dateEnd = (DateTime)Calendarx_EndtDate.SelectedDate;

            switch (ComboBox_Categori.SelectedValue)
            {
                case CategoriEnum.Income:
                    ExcelReportIncome();
                    break;
                case CategoriEnum.Expense:
                    ExcelReportExpense();
                    break;
            }
        }

        private void ExcelReportIncome()
        {
            var allUsers = _context.User.ToList().OrderBy(u => u.Login).ToList();

            var application = new Excel.Application();
            application.SheetsInNewWorkbook = allUsers.Count();
            Excel.Workbook workbook = application.Workbooks.Add(Type.Missing);
            for (int i = 0; i < allUsers.Count(); i++)
            {
                int startRowIndex = 1;
                Excel.Worksheet worksheet = application.Worksheets.Item[i + 1];
                worksheet.Name = allUsers[i].Login;

                worksheet.Cells[1][startRowIndex] = "Описание";
                worksheet.Cells[2][startRowIndex] = "Дата";
                worksheet.Cells[3][startRowIndex] = "Сумма"; 
                Excel.Range columlHeaderRange = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[3][1]];
                columlHeaderRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                columlHeaderRange.Font.Bold = true;
                startRowIndex++;

                var userIncome = allUsers[i].UserIncome.Where(u => u.Data >= dateStart && u.Data <= dateEnd).OrderBy(u => u.Data);

                foreach (var income in userIncome)
                {
                    worksheet.Cells[1][startRowIndex] = income.Description;
                    worksheet.Cells[2][startRowIndex] = income.Data.ToString("dd.MM.yyyy");
                    worksheet.Cells[3][startRowIndex] = income.Amount;

                    startRowIndex++;
                }

                Excel.Range sumRange = worksheet.Range[worksheet.Cells[1][startRowIndex], worksheet.Cells[2][startRowIndex]];
                sumRange.Merge();
                sumRange.Value = "ИТОГО:";
                sumRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                worksheet.Cells[3][startRowIndex].Formula = $"=SUM(C{2}:"+$"C{startRowIndex - 1})";
                sumRange.Font.Bold = worksheet.Cells[3][startRowIndex].Font.Bold = true;
                startRowIndex++;

                Excel.Range rangeBorders = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[3][startRowIndex - 1]];
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle 
                    = rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle 
                    = rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle 
                    = rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle 
                    = rangeBorders.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle 
                    = rangeBorders.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle 
                    = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();
            }
            application.Visible = true;

            MessageBox.Show("Отчет по доходу сформирован");
        }

        private void ExcelReportExpense()
        {
            var allUsers = _context.User.ToList().OrderBy(u => u.Login).ToList();

            var application = new Excel.Application();
            application.SheetsInNewWorkbook = allUsers.Count();
            Excel.Workbook workbook = application.Workbooks.Add(Type.Missing);
            for (int i = 0; i < allUsers.Count(); i++)
            {
                int startRowIndex = 1;
                Excel.Worksheet worksheet = application.Worksheets.Item[i + 1];
                worksheet.Name = allUsers[i].Login;

                worksheet.Cells[1][startRowIndex] = "Описание";
                worksheet.Cells[2][startRowIndex] = "Дата";
                worksheet.Cells[3][startRowIndex] = "Сумма"; 
                Excel.Range columlHeaderRange = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[3][1]];
                columlHeaderRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                columlHeaderRange.Font.Bold = true;
                startRowIndex++;

                var userExpense = allUsers[i].UserExpense.Where(u => u.Data >= dateStart && u.Data <= dateEnd).OrderBy(u => u.Data);

                foreach (var expense in userExpense)
                {
                    worksheet.Cells[1][startRowIndex] = expense.Description;
                    worksheet.Cells[2][startRowIndex] = expense.Data.ToString("dd.MM.yyyy");
                    worksheet.Cells[3][startRowIndex] = expense.Amount;

                    startRowIndex++;
                }

                Excel.Range sumRange = worksheet.Range[worksheet.Cells[1][startRowIndex], worksheet.Cells[2][startRowIndex]];
                sumRange.Merge();
                sumRange.Value = "ИТОГО:";
                sumRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                worksheet.Cells[3][startRowIndex].Formula = $"=SUM(C{2}:" + $"C{startRowIndex - 1})";
                sumRange.Font.Bold = worksheet.Cells[3][startRowIndex].Font.Bold = true;
                startRowIndex++;

                Excel.Range rangeBorders = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[3][startRowIndex - 1]];
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle
                    = rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle
                    = rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle
                    = rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle
                    = rangeBorders.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle
                    = rangeBorders.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle
                    = Excel.XlLineStyle.xlContinuous;

                worksheet.Columns.AutoFit();
            }
            application.Visible = true;

            MessageBox.Show("Отчет по расходу сформирован");
        }
    }
}
