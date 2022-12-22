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
using Word = Microsoft.Office.Interop.Word;

namespace PR30
{
    public partial class WordReports : System.Windows.Controls.Page
    {
        private PR29DatabaseEntities _context = new PR29DatabaseEntities();

        private DateTime dateStart { get; set; }
        private DateTime dateEnd { get; set; }

        public WordReports()
        {
            InitializeComponent();

            Title = "Формирование отчетов Word";

            foreach (CategoriEnum categor in Enum.GetValues(typeof(CategoriEnum)))
            {
                ComboBox_Categori.Items.Add(categor);
            }
            ComboBox_Categori.SelectedValue = CategoriEnum.Income;
        }

        private void Btn_WordReports_Click(object sender, RoutedEventArgs e)
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
                    WordReportIncome();
                    break;
                case CategoriEnum.Expense:
                    WordReportExpense();
                    break;
            }
        }


        private void WordReportIncome()
        {
            var allUsers = _context.User.ToList();

            var application = new Word.Application();
            Word.Document document = application.Documents.Add();

            foreach (var user in allUsers)
            {
                Word.Paragraph userParagraph = document.Paragraphs.Add();
                Word.Range userRange = userParagraph.Range;
                userRange.Text = user.Login;
                userParagraph.set_Style("Заголовок");
                userRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                userRange.InsertParagraphAfter();
                document.Paragraphs.Add(); //Пустая строка

                Word.Paragraph tableParagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table paymentsTable = document.Tables.Add(tableRange, 
                    user.UserIncome.Where(u => u.Data >= dateStart && u.Data <= dateEnd).Count() + 1, 3);
                paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle =
                         Word.WdLineStyle.wdLineStyleSingle;
                paymentsTable.Range.Cells.VerticalAlignment
                    = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                Word.Range cellRange;

                cellRange = paymentsTable.Cell(1, 1).Range;
                cellRange.Text = "Дата";
                cellRange = paymentsTable.Cell(1, 2).Range;
                cellRange.Text = "Сумма";
                cellRange = paymentsTable.Cell(1, 3).Range;
                cellRange.Text = "Описание";

                paymentsTable.Rows[1].Range.Font.Name = "Times New Roman";
                paymentsTable.Rows[1].Range.Font.Size = 14;
                paymentsTable.Rows[1].Range.Bold = 1;
                paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                var income = user.UserIncome.Where(u => u.Data >= dateStart && u.Data <= dateEnd).OrderBy(u => u.Data).ToList();

                for (int i = 0; i < income.Count(); i++)
                {
                    var currentCategory = income[i];
                    cellRange = paymentsTable.Cell(i + 2, 1).Range;
                    cellRange.Text = currentCategory.Data.ToString();
                    cellRange.Font.Name = "Times New Roman";
                    cellRange.Font.Size = 12;

                    cellRange = paymentsTable.Cell(i + 2, 2).Range;
                    cellRange.Text = currentCategory.Amount.ToString() + " руб.";
                    cellRange.Font.Name = "Times New Roman";
                    cellRange.Font.Size = 12;

                    cellRange = paymentsTable.Cell(i + 2, 3).Range;
                    cellRange.Text = currentCategory.Description;
                    cellRange.Font.Name = "Times New Roman";
                    cellRange.Font.Size = 12;
                } //завершение цикла по строкам таблицы
                document.Paragraphs.Add(); //пустая строка

                if (user != allUsers.LastOrDefault())
                    document.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);

                application.Visible = true;
            }
        }

        private void WordReportExpense()
        {
            var allUsers = _context.User.ToList();

            var application = new Word.Application();
            Word.Document document = application.Documents.Add();

            foreach (var user in allUsers)
            {
                Word.Paragraph userParagraph = document.Paragraphs.Add();
                Word.Range userRange = userParagraph.Range;
                userRange.Text = user.Login;
                userParagraph.set_Style("Заголовок");
                userRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                userRange.InsertParagraphAfter();
                document.Paragraphs.Add(); //Пустая строка

                Word.Paragraph tableParagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table paymentsTable = document.Tables.Add(tableRange, 
                    user.UserExpense.Where(u => u.Data >= dateStart && u.Data <= dateEnd).Count() + 1, 3);
                paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle =
                         Word.WdLineStyle.wdLineStyleSingle;
                paymentsTable.Range.Cells.VerticalAlignment
                    = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                Word.Range cellRange;

                cellRange = paymentsTable.Cell(1, 1).Range;
                cellRange.Text = "Дата";
                cellRange = paymentsTable.Cell(1, 2).Range;
                cellRange.Text = "Сумма";
                cellRange = paymentsTable.Cell(1, 3).Range;
                cellRange.Text = "Описание";

                paymentsTable.Rows[1].Range.Font.Name = "Times New Roman";
                paymentsTable.Rows[1].Range.Font.Size = 14;
                paymentsTable.Rows[1].Range.Bold = 1;
                paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                var expense = user.UserExpense.Where(u => u.Data >= dateStart && u.Data <= dateEnd).OrderBy(u => u.Data).ToList();

                for (int i = 0; i < expense.Count(); i++)
                {
                    var currentCategory = expense[i];
                    cellRange = paymentsTable.Cell(i + 2, 1).Range;
                    cellRange.Text = currentCategory.Data.ToString();
                    cellRange.Font.Name = "Times New Roman";
                    cellRange.Font.Size = 12;

                    cellRange = paymentsTable.Cell(i + 2, 2).Range;
                    cellRange.Text = currentCategory.Amount.ToString() + " руб.";
                    cellRange.Font.Name = "Times New Roman";
                    cellRange.Font.Size = 12;

                    cellRange = paymentsTable.Cell(i + 2, 3).Range;
                    cellRange.Text = currentCategory.Description;
                    cellRange.Font.Name = "Times New Roman";
                    cellRange.Font.Size = 12;
                } //завершение цикла по строкам таблицы
                document.Paragraphs.Add(); //пустая строка

                if (user != allUsers.LastOrDefault())
                    document.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);

                application.Visible = true;
            }
        }

    }

}
