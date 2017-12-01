using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;

namespace Budgeteer.Desktop.WPF
{
  public partial class MainWindow
  {
    private List<KeyValuePair<string, double>> _incomeData;
    private List<KeyValuePair<string, double>> _spendingData;

    private void CalculateBudgetBalance()
    {
      var incomeTotal = _incomeData.Sum(incomePair => incomePair.Value);
      var spendingTotal = _spendingData.Sum(spendingPair => spendingPair.Value);
      var budgetBalance = incomeTotal - spendingTotal;

      ChartOverviewLeft.Title = $"Overall Budget Balance: {budgetBalance:C}";
    }

    private void LoadIncomeData()
    {
      var incomeRecords = _creditQuery.ToList();

      var incomeRecordsByMonth = from record in incomeRecords
                                 where record.Date.AddMonths(13) > DateTime.Today
                                 group record.Amount by record.Date.ToString("yyyy/MM")
                                 into monthlyRecords
                                 orderby monthlyRecords.Key
                                 select monthlyRecords;

      _incomeData = new List<KeyValuePair<string, double>>();
      foreach(var period in incomeRecordsByMonth)
        _incomeData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

      ((ColumnSeries)ChartOverviewLeft.Series[0]).ItemsSource = _incomeData;
    }

    private void LoadSpendingData()
    {
      var spendingRecords = _debitQuery.ToList();

      var spendingRecordsByMonth = from record in spendingRecords
                                   where record.Date.AddMonths(13) > DateTime.Today
                                   group record.Amount by record.Date.ToString("yyyy/MM")
                                   into monthlyRecords
                                   orderby monthlyRecords.Key
                                   select monthlyRecords;

      _spendingData = new List<KeyValuePair<string, double>>();
      foreach(var period in spendingRecordsByMonth)
        _spendingData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

      ((ColumnSeries)ChartOverviewLeft.Series[1]).ItemsSource = _spendingData;
    }

    private void LoadSpendingDistributionData()
    {
      var spendingRecords = _debitQuery.ToList();

      var spendingDistributionData = new List<KeyValuePair<string, double>>();
      foreach(var spendingCategory in Debit.DebitCategories)
      {
        var categoryTotal = (from record in spendingRecords
                             where record.Category == spendingCategory && record.Date.AddMonths(7) > DateTime.Today
                             select record.Amount).Sum();

        if(categoryTotal > 0)
          spendingDistributionData.Add(new KeyValuePair<string, double>(spendingCategory, categoryTotal));
      }
            ((PieSeries)ChartOverviewRight.Series[0]).ItemsSource = spendingDistributionData;
    }

    private void ReloadOverviewData()
    {
      LoadIncomeData();
      LoadSpendingData();
      CalculateBudgetBalance();
      LoadSpendingDistributionData();
    }

    private void SetupOverviewTab()
    {
      ComboBoxAddPerson.ItemsSource = Transaction.People;
      ComboBoxAddCategory.ItemsSource = Debit.DebitCategories;
      DataGridOverview.ItemsSource = _records;

      ReloadOverviewData();
      SortDataGridByDate();
    }

    private void SortDataGridByDate()
    {
      var gridData = CollectionViewSource.GetDefaultView(DataGridOverview.ItemsSource);
      gridData.SortDescriptions.Clear();
      gridData.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
      DataGridOverview.Columns[0].SortDirection = ListSortDirection.Descending;
    }
  }
}
