using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls.DataVisualization.Charting;

namespace Budgeteer_WPF_Files
{
    public partial class MainWindow
    {
        private void SetupIncomeTab()
        {
            ComboBoxIncomePerson.ItemsSource = Transaction.People;
            DatePickerIncomeFrom.SelectedDate = DateTime.Today.AddMonths(-12);
            DatePickerIncomeFrom.SelectedDateChanged += IncomeFilterChanged;
            DatePickerIncomeUntil.SelectedDate = DateTime.Today;
            DatePickerIncomeUntil.SelectedDateChanged += IncomeFilterChanged;
            ComboBoxIncomeCategory.ItemsSource = Credit.CreditCategories;

            ReloadIncomeData();
        }

        private void ReloadIncomeData()
        {
            LoadIncomeByData();
            LoadIncomeDistributionOfData();

            LoadIncomeForData();
            LoadIncomeDistributionForData();
        }

        private void IncomeFilterChanged(object sender, EventArgs e)
        {
            ReloadIncomeData();
        }

        private void LoadIncomeByData()
        {
            List<Credit> incomeRecords = creditQuery.ToList();

            IOrderedEnumerable<IGrouping<string, double>> incomeRecordsByMonth = from record in incomeRecords
                where
                    record.Person == ComboBoxIncomePerson.Text && record.Date >= DatePickerIncomeFrom.DisplayDate &&
                    record.Date <= DatePickerIncomeUntil.DisplayDate
                group record.Amount by record.Date.ToString("yyyy/MM")
                into monthlyRecords
                orderby monthlyRecords.Key
                select monthlyRecords;

            List<KeyValuePair<string, double>> incomeData = new List<KeyValuePair<string, double>>();
            foreach (IGrouping<string, double> period in incomeRecordsByMonth)
                incomeData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

            ChartIncomeTopLeft.Title = $"Income of {ComboBoxIncomePerson.Text}";
            ((ColumnSeries) ChartIncomeTopLeft.Series[0]).ItemsSource = incomeData;
        }

        private void LoadIncomeDistributionOfData()
        {
            List<Credit> incomeRecords = creditQuery.ToList();

            List<KeyValuePair<string, double>> incomeDistributionData = new List<KeyValuePair<string, double>>();
            foreach (string incomeCategory in Credit.CreditCategories)
            {
                double categoryTotal = (from record in incomeRecords
                    where
                        record.Category == incomeCategory && record.Person == ComboBoxIncomePerson.Text &&
                        record.Date >= DatePickerIncomeFrom.DisplayDate &&
                        record.Date <= DatePickerIncomeUntil.DisplayDate
                    select record.Amount).Sum();

                if (categoryTotal > 0)
                    incomeDistributionData.Add(new KeyValuePair<string, double>(incomeCategory, categoryTotal));
            }
            ChartIncomeBottomLeft.Title = $"Income Distribution of {ComboBoxIncomePerson.Text}";
            ((PieSeries) ChartIncomeBottomLeft.Series[0]).ItemsSource = incomeDistributionData;
        }

        private void LoadIncomeForData()
        {
            List<Credit> incomeRecords = creditQuery.ToList();

            IOrderedEnumerable<IGrouping<string, double>> incomeRecordsByMonth = from record in incomeRecords
                where
                    record.Category == ComboBoxIncomeCategory.Text &&
                    record.Date >= DatePickerIncomeFrom.DisplayDate &&
                    record.Date <= DatePickerIncomeUntil.DisplayDate
                group record.Amount by record.Date.ToString("yyyy/MM")
                into monthlyRecords
                orderby monthlyRecords.Key
                select monthlyRecords;

            List<KeyValuePair<string, double>> incomeData = new List<KeyValuePair<string, double>>();
            foreach (IGrouping<string, double> period in incomeRecordsByMonth)
                incomeData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

            ChartIncomeTopRight.Title = $"Income from {ComboBoxIncomeCategory.Text}";
            ((ColumnSeries) ChartIncomeTopRight.Series[0]).ItemsSource = incomeData;
        }

        private void LoadIncomeDistributionForData()
        {
            List<Credit> incomeRecords = creditQuery.ToList();

            List<KeyValuePair<string, double>> incomeDistributionData = new List<KeyValuePair<string, double>>();
            foreach (string incomePerson in Transaction.People)
            {
                double categoryTotal = (from record in incomeRecords
                    where
                        record.Person == incomePerson && record.Category == ComboBoxIncomeCategory.Text &&
                        record.Date >= DatePickerIncomeFrom.DisplayDate &&
                        record.Date <= DatePickerIncomeUntil.DisplayDate
                    select record.Amount).Sum();

                if (categoryTotal > 0)
                    incomeDistributionData.Add(new KeyValuePair<string, double>(incomePerson, categoryTotal));
            }
            ChartIncomeBottomRight.Title = $"Income Distribution from {ComboBoxIncomeCategory.Text}";
            ((PieSeries) ChartIncomeBottomRight.Series[0]).ItemsSource = incomeDistributionData;
        }
    }
}