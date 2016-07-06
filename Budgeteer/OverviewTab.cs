﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;

namespace Budgeteer
{
    public partial class MainWindow
    {
        private void SetupOverviewTab()
        {
            ComboBoxAddPerson.ItemsSource = Transaction.People;
            ComboBoxAddCategory.ItemsSource = Debit.DebitCategories;
            DataGridOverview.ItemsSource = _records;

            ReloadOverviewData();
            SortDataGridByDate();
        }

        private void ReloadOverviewData()
        {
            LoadIncomeData();
            LoadSpendingData();
            LoadSpendingDistributionData();
        }

        private void LoadIncomeData()
        {
            List<Credit> incomeRecords = _creditQuery.ToList();

            IOrderedEnumerable<IGrouping<string, double>> incomeRecordsByMonth = from record in incomeRecords
                where record.Date.AddMonths(7) > DateTime.Today
                group record.Amount by record.Date.ToString("yyyy/MM")
                into monthlyRecords
                orderby monthlyRecords.Key
                select monthlyRecords;

            List<KeyValuePair<string, double>> incomeData = new List<KeyValuePair<string, double>>();
            foreach (IGrouping<string, double> period in incomeRecordsByMonth)
                incomeData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

            ((ColumnSeries) ChartOverviewLeft.Series[0]).ItemsSource = incomeData;
        }

        private void LoadSpendingData()
        {
            List<Debit> spendingRecords = _debitQuery.ToList();

            IOrderedEnumerable<IGrouping<string, double>> spendingRecordsByMonth = from record in spendingRecords
                where record.Date.AddMonths(7) > DateTime.Today
                group record.Amount by record.Date.ToString("yyyy/MM")
                into monthlyRecords
                orderby monthlyRecords.Key
                select monthlyRecords;

            List<KeyValuePair<string, double>> spendingData = new List<KeyValuePair<string, double>>();
            foreach (IGrouping<string, double> period in spendingRecordsByMonth)
                spendingData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

            ((ColumnSeries) ChartOverviewLeft.Series[1]).ItemsSource = spendingData;
        }

        private void LoadSpendingDistributionData()
        {
            List<Debit> spendingRecords = _debitQuery.ToList();

            List<KeyValuePair<string, double>> spendingDistributionData = new List<KeyValuePair<string, double>>();
            foreach (string spendingCategory in Debit.DebitCategories)
            {
                double categoryTotal = (from record in spendingRecords
                    where record.Category == spendingCategory && record.Date.AddMonths(7) > DateTime.Today
                    select record.Amount).Sum();

                if (categoryTotal > 0)
                    spendingDistributionData.Add(new KeyValuePair<string, double>(spendingCategory, categoryTotal));
            }
            ((PieSeries) ChartOverviewRight.Series[0]).ItemsSource = spendingDistributionData;
        }

        private void SortDataGridByDate()
        {
            ICollectionView gridData = CollectionViewSource.GetDefaultView(DataGridOverview.ItemsSource);
            gridData.SortDescriptions.Clear();
            gridData.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
            DataGridOverview.Columns[0].SortDirection = ListSortDirection.Descending;
        }
    }
}