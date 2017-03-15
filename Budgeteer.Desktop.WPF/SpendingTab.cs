using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

namespace Budgeteer.Desktop.WPF
{
    public partial class MainWindow
    {
        private void SetupSpendingTab()
        {
            ComboBoxSpendingPerson.ItemsSource = Transaction.People;
            DatePickerSpendingFrom.SelectedDate = DateTime.Today.AddMonths(-12);
            DatePickerSpendingFrom.SelectedDateChanged += SpendingDateChanged;
            DatePickerSpendingUntil.SelectedDate = DateTime.Today;
            DatePickerSpendingUntil.SelectedDateChanged += SpendingDateChanged;
            ComboBoxSpendingCategory.ItemsSource = Debit.DebitCategories;

            ReloadSpendingData();
        }

        private void ReloadSpendingData()
        {
            ReloadPersonSpendingData();
            ReloadCategorySpendingData();
        }

        private void ReloadPersonSpendingData()
        {
            LoadSpendingByData();
            LoadSpendingDistributionOfData();
        }

        private void ReloadCategorySpendingData()
        {
            LoadSpendingForData();
            LoadSpendingDistributionForData();
        }

        private void ComboBoxSpendingPerson_DropDownClosed(object sender, EventArgs e)
        {
            ReloadPersonSpendingData();
        }

        private void SpendingDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadSpendingData();
        }

        private void ComboBoxSpendingCategory_DropDownClosed(object sender, EventArgs e)
        {
            ReloadCategorySpendingData();
        }

        private void LoadSpendingByData()
        {
            var spendingRecords = _debitQuery.ToList();

            var spendingRecordsByMonth = from record in spendingRecords
                where
                record.Person == ComboBoxSpendingPerson.Text && record.Date >= DatePickerSpendingFrom.DisplayDate &&
                record.Date <= DatePickerSpendingUntil.DisplayDate
                group record.Amount by record.Date.ToString("yyyy/MM")
                into monthlyRecords
                orderby monthlyRecords.Key
                select monthlyRecords;

            var spendingData = new List<KeyValuePair<string, double>>();
            foreach (var period in spendingRecordsByMonth)
                spendingData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

            ChartSpendingTopLeft.Title = $"Spending of {ComboBoxSpendingPerson.Text}";
            ((ColumnSeries) ChartSpendingTopLeft.Series[0]).ItemsSource = spendingData;
        }

        private void LoadSpendingDistributionOfData()
        {
            var spendingRecords = _debitQuery.ToList();

            var spendingDistributionData = new List<KeyValuePair<string, double>>();
            foreach (var spendingCategory in Debit.DebitCategories)
            {
                var categoryTotal = (from record in spendingRecords
                    where
                    record.Category == spendingCategory && record.Person == ComboBoxSpendingPerson.Text &&
                    record.Date >= DatePickerSpendingFrom.DisplayDate &&
                    record.Date <= DatePickerSpendingUntil.DisplayDate
                    select record.Amount).Sum();

                if (categoryTotal > 0)
                    spendingDistributionData.Add(new KeyValuePair<string, double>(spendingCategory, categoryTotal));
            }
            ChartSpendingBottomLeft.Title = $"Spending Distribution of {ComboBoxSpendingPerson.Text}";
            ((PieSeries) ChartSpendingBottomLeft.Series[0]).ItemsSource = spendingDistributionData;
        }

        private void LoadSpendingForData()
        {
            var spendingRecords = _debitQuery.ToList();

            var spendingRecordsByMonth = from record in spendingRecords
                where
                record.Category == ComboBoxSpendingCategory.Text &&
                record.Date >= DatePickerSpendingFrom.DisplayDate &&
                record.Date <= DatePickerSpendingUntil.DisplayDate
                group record.Amount by record.Date.ToString("yyyy/MM")
                into monthlyRecords
                orderby monthlyRecords.Key
                select monthlyRecords;

            var spendingData = new List<KeyValuePair<string, double>>();
            foreach (var period in spendingRecordsByMonth)
                spendingData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

            ChartSpendingTopRight.Title = $"Spending for {ComboBoxSpendingCategory.Text}";
            ((ColumnSeries) ChartSpendingTopRight.Series[0]).ItemsSource = spendingData;
        }

        private void LoadSpendingDistributionForData()
        {
            var spendingRecords = _debitQuery.ToList();

            var spendingDistributionData = new List<KeyValuePair<string, double>>();
            foreach (var spendingPerson in Transaction.People)
            {
                var categoryTotal = (from record in spendingRecords
                    where
                    record.Person == spendingPerson && record.Category == ComboBoxSpendingCategory.Text &&
                    record.Date >= DatePickerSpendingFrom.DisplayDate &&
                    record.Date <= DatePickerSpendingUntil.DisplayDate
                    select record.Amount).Sum();

                if (categoryTotal > 0)
                    spendingDistributionData.Add(new KeyValuePair<string, double>(spendingPerson, categoryTotal));
            }
            ChartSpendingBottomRight.Title = $"Spending Distribution for {ComboBoxSpendingCategory.Text}";
            ((PieSeries) ChartSpendingBottomRight.Series[0]).ItemsSource = spendingDistributionData;
        }
    }
}