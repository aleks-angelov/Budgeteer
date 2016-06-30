using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

namespace Budgeteer
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
            List<Debit> spendingRecords = _debitQuery.ToList();

            IOrderedEnumerable<IGrouping<string, double>> spendingRecordsByMonth = from record in spendingRecords
                where
                    record.Person == ComboBoxSpendingPerson.Text && record.Date >= DatePickerSpendingFrom.DisplayDate &&
                    record.Date <= DatePickerSpendingUntil.DisplayDate
                group record.Amount by record.Date.ToString("yyyy/MM")
                into monthlyRecords
                orderby monthlyRecords.Key
                select monthlyRecords;

            List<KeyValuePair<string, double>> spendingData = new List<KeyValuePair<string, double>>();
            foreach (IGrouping<string, double> period in spendingRecordsByMonth)
                spendingData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

            ChartSpendingTopLeft.Title = $"Spending of {ComboBoxSpendingPerson.Text}";
            ((ColumnSeries) ChartSpendingTopLeft.Series[0]).ItemsSource = spendingData;
        }

        private void LoadSpendingDistributionOfData()
        {
            List<Debit> spendingRecords = _debitQuery.ToList();

            List<KeyValuePair<string, double>> spendingDistributionData = new List<KeyValuePair<string, double>>();
            foreach (string spendingCategory in Debit.DebitCategories)
            {
                double categoryTotal = (from record in spendingRecords
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
            List<Debit> spendingRecords = _debitQuery.ToList();

            IOrderedEnumerable<IGrouping<string, double>> spendingRecordsByMonth = from record in spendingRecords
                where
                    record.Category == ComboBoxSpendingCategory.Text &&
                    record.Date >= DatePickerSpendingFrom.DisplayDate &&
                    record.Date <= DatePickerSpendingUntil.DisplayDate
                group record.Amount by record.Date.ToString("yyyy/MM")
                into monthlyRecords
                orderby monthlyRecords.Key
                select monthlyRecords;

            List<KeyValuePair<string, double>> spendingData = new List<KeyValuePair<string, double>>();
            foreach (IGrouping<string, double> period in spendingRecordsByMonth)
                spendingData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

            ChartSpendingTopRight.Title = $"Spending for {ComboBoxSpendingCategory.Text}";
            ((ColumnSeries) ChartSpendingTopRight.Series[0]).ItemsSource = spendingData;
        }

        private void LoadSpendingDistributionForData()
        {
            List<Debit> spendingRecords = _debitQuery.ToList();

            List<KeyValuePair<string, double>> spendingDistributionData = new List<KeyValuePair<string, double>>();
            foreach (string spendingPerson in Transaction.People)
            {
                double categoryTotal = (from record in spendingRecords
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