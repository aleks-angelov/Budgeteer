using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;

namespace Budgeteer.Desktop.WPF
{
  public partial class MainWindow
  {
    private void ComboBoxIncomeCategory_DropDownClosed(object sender, EventArgs e)
    {
      ReloadCategoryIncomeData();
    }

    private void ComboBoxIncomePerson_DropDownClosed(object sender, EventArgs e)
    {
      ReloadPersonIncomeData();
    }

    private void IncomeDateChanged(object sender, SelectionChangedEventArgs e)
    {
      ReloadIncomeData();
    }

    private void LoadIncomeByData()
    {
      var incomeRecords = _creditQuery.ToList();

      var incomeRecordsByMonth = from record in incomeRecords
                                 where
                                 record.Person == ComboBoxIncomePerson.Text && record.Date >= DatePickerIncomeFrom.DisplayDate &&
                                 record.Date <= DatePickerIncomeUntil.DisplayDate
                                 group record.Amount by record.Date.ToString("yyyy/MM")
                                 into monthlyRecords
                                 orderby monthlyRecords.Key
                                 select monthlyRecords;

      var incomeData = new List<KeyValuePair<string, double>>();
      foreach(var period in incomeRecordsByMonth)
        incomeData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

      ChartIncomeTopLeft.Title = $"Income of {ComboBoxIncomePerson.Text}";
      ((ColumnSeries)ChartIncomeTopLeft.Series[0]).ItemsSource = incomeData;
    }

    private void LoadIncomeDistributionForData()
    {
      var incomeRecords = _creditQuery.ToList();

      var incomeDistributionData = new List<KeyValuePair<string, double>>();
      foreach(var incomePerson in Transaction.People)
      {
        var categoryTotal = (from record in incomeRecords
                             where
                             record.Person == incomePerson && record.Category == ComboBoxIncomeCategory.Text &&
                             record.Date >= DatePickerIncomeFrom.DisplayDate &&
                             record.Date <= DatePickerIncomeUntil.DisplayDate
                             select record.Amount).Sum();

        if(categoryTotal > 0)
          incomeDistributionData.Add(new KeyValuePair<string, double>(incomePerson, categoryTotal));
      }
      ChartIncomeBottomRight.Title = $"Income Distribution from {ComboBoxIncomeCategory.Text}";
      ((PieSeries)ChartIncomeBottomRight.Series[0]).ItemsSource = incomeDistributionData;
    }

    private void LoadIncomeDistributionOfData()
    {
      var incomeRecords = _creditQuery.ToList();

      var incomeDistributionData = new List<KeyValuePair<string, double>>();
      foreach(var incomeCategory in Credit.CreditCategories)
      {
        var categoryTotal = (from record in incomeRecords
                             where
                             record.Category == incomeCategory && record.Person == ComboBoxIncomePerson.Text &&
                             record.Date >= DatePickerIncomeFrom.DisplayDate &&
                             record.Date <= DatePickerIncomeUntil.DisplayDate
                             select record.Amount).Sum();

        if(categoryTotal > 0)
          incomeDistributionData.Add(new KeyValuePair<string, double>(incomeCategory, categoryTotal));
      }
      ChartIncomeBottomLeft.Title = $"Income Distribution of {ComboBoxIncomePerson.Text}";
      ((PieSeries)ChartIncomeBottomLeft.Series[0]).ItemsSource = incomeDistributionData;
    }

    private void LoadIncomeForData()
    {
      var incomeRecords = _creditQuery.ToList();

      var incomeRecordsByMonth = from record in incomeRecords
                                 where
                                 record.Category == ComboBoxIncomeCategory.Text &&
                                 record.Date >= DatePickerIncomeFrom.DisplayDate &&
                                 record.Date <= DatePickerIncomeUntil.DisplayDate
                                 group record.Amount by record.Date.ToString("yyyy/MM")
                                 into monthlyRecords
                                 orderby monthlyRecords.Key
                                 select monthlyRecords;

      var incomeData = new List<KeyValuePair<string, double>>();
      foreach(var period in incomeRecordsByMonth)
        incomeData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

      ChartIncomeTopRight.Title = $"Income from {ComboBoxIncomeCategory.Text}";
      ((ColumnSeries)ChartIncomeTopRight.Series[0]).ItemsSource = incomeData;
    }

    private void ReloadCategoryIncomeData()
    {
      LoadIncomeForData();
      LoadIncomeDistributionForData();
    }

    private void ReloadIncomeData()
    {
      ReloadPersonIncomeData();
      ReloadCategoryIncomeData();
    }

    private void ReloadPersonIncomeData()
    {
      LoadIncomeByData();
      LoadIncomeDistributionOfData();
    }

    private void SetupIncomeTab()
    {
      ComboBoxIncomePerson.ItemsSource = Transaction.People;
      DatePickerIncomeFrom.SelectedDate = DateTime.Today.AddMonths(-12);
      DatePickerIncomeFrom.SelectedDateChanged += IncomeDateChanged;
      DatePickerIncomeUntil.SelectedDate = DateTime.Today;
      DatePickerIncomeUntil.SelectedDateChanged += IncomeDateChanged;
      ComboBoxIncomeCategory.ItemsSource = Credit.CreditCategories;

      ReloadIncomeData();
    }
  }
}
