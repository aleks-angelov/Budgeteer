using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace Budgeteer_WPF_Files
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ObservableCollection<Transaction> records = new ObservableCollection<Transaction>
        {
            new Debit(DateTime.Today.AddMonths(-12), "Aleks Angelov", "Groceries", 100.05, "Lunch and dinner"),
            new Debit(DateTime.Today.AddMonths(-11), "Aleks Angelov", "Groceries", 50.05, "Lunch and dinner"),
            new Debit(DateTime.Today.AddMonths(-10), "Aleks Angelov", "Groceries", 40.05, "Lunch and dinner"),
            new Debit(DateTime.Today.AddMonths(-9), "Aleks Angelov", "Groceries", 10.05, "Lunch and dinner"),
            new Debit(DateTime.Today.AddMonths(-8), "Aleks Angelov", "Groceries", 20.05, "Lunch and dinner"),
            new Debit(DateTime.Today.AddMonths(-7), "Aleks Angelov", "Groceries", 30.05, "Lunch and dinner"),
            new Debit(DateTime.Today.AddMonths(-6), "Aleks Angelov", "Groceries", 160.05, "Lunch and dinner"),
            new Debit(DateTime.Today.AddMonths(-5), "Aleks Angelov", "Groceries", 150.05, "Lunch and dinner"),
            new Debit(DateTime.Today.AddMonths(-4), "Aleks Angelov", "Groceries", 140.05, "Lunch and dinner"),
            new Debit(DateTime.Today.AddMonths(-3), "Aleks Angelov", "Groceries", 110.05, "Lunch and dinner"),
            new Debit(DateTime.Today.AddMonths(-2), "Aleks Angelov", "Groceries", 120.05, "Lunch and dinner"),
            new Debit(DateTime.Today.AddMonths(-1), "Aleks Angelov", "Groceries", 130.05, "Lunch and dinner"),
            new Debit(DateTime.Today, "Aleks Angelov", "Groceries", 60.05, "Lunch and dinner"),

            new Credit(DateTime.Today.AddMonths(-12), "Aleks Angelov", "Salary", 109.05, "Lunch and dinner"),
            new Credit(DateTime.Today.AddMonths(-11), "Aleks Angelov", "Salary", 58.05, "Lunch and dinner"),
            new Credit(DateTime.Today.AddMonths(-10), "Aleks Angelov", "Salary", 47.05, "Lunch and dinner"),
            new Credit(DateTime.Today.AddMonths(-9), "Aleks Angelov", "Salary", 16.05, "Lunch and dinner"),
            new Credit(DateTime.Today.AddMonths(-8), "Aleks Angelov", "Salary", 25.05, "Lunch and dinner"),
            new Credit(DateTime.Today.AddMonths(-7), "Aleks Angelov", "Salary", 34.05, "Lunch and dinner"),
            new Credit(DateTime.Today.AddMonths(-6), "Aleks Angelov", "Salary", 164.05, "Lunch and dinner"),
            new Credit(DateTime.Today.AddMonths(-5), "Aleks Angelov", "Salary", 155.05, "Lunch and dinner"),
            new Credit(DateTime.Today.AddMonths(-4), "Aleks Angelov", "Salary", 146.05, "Lunch and dinner"),
            new Credit(DateTime.Today.AddMonths(-3), "Aleks Angelov", "Salary", 117.05, "Lunch and dinner"),
            new Credit(DateTime.Today.AddMonths(-2), "Aleks Angelov", "Salary", 128.05, "Lunch and dinner"),
            new Credit(DateTime.Today.AddMonths(-1), "Aleks Angelov", "Salary", 139.05, "Lunch and dinner"),
            new Credit(DateTime.Today, "Aleks Angelov", "Salary", 60.05, "Lunch and dinner")
        };

        private static readonly IEnumerable<Debit> debitQuery = from record in records
            where record.Type == "Debit"
            select record as Debit;

        private static readonly IEnumerable<Credit> creditQuery = from record in records
            where record.Type == "Credit"
            select record as Credit;

        public MainWindow()
        {
            InitializeComponent();

            records.CollectionChanged += RecordsOnCollectionChanged;
            SetupOverviewTab();
        }

        private void RecordsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            LoadBudgetBalanceData();
            LoadSpendingDistributionData();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBudgetBalanceData();
            LoadSpendingDistributionData();
        }

        private void SetupOverviewTab()
        {
            ComboBoxAddPerson.ItemsSource = Transaction.People;
            ComboBoxAddCategory.ItemsSource = Debit.DebitCategories;
            DataGridOverview.ItemsSource = records;
        }

        private void LoadBudgetBalanceData()
        {
            LoadIncomeData();
            LoadSpendingData();
        }

        private void LoadIncomeData()
        {
            List<Credit> incomeRecords = creditQuery.ToList();

            IOrderedEnumerable<IGrouping<string, double>> incomeRecordsByMonth = from record in incomeRecords
                group record.Amount by record.Date.ToString("yyyy/MM")
                into monthlyRecords
                orderby monthlyRecords.Key
                select monthlyRecords;

            List<KeyValuePair<string, double>> incomeData = new List<KeyValuePair<string, double>>();
            foreach (IGrouping<string, double> period in incomeRecordsByMonth)
                incomeData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

            ((LineSeries) ChartOverviewLeft.Series[0]).ItemsSource = incomeData;
        }

        private void LoadSpendingData()
        {
            List<Debit> spendingRecords = debitQuery.ToList();

            IOrderedEnumerable<IGrouping<string, double>> spendingRecordsByMonth = from record in spendingRecords
                group record.Amount by record.Date.ToString("yyyy/MM")
                into monthlyRecords
                orderby monthlyRecords.Key
                select monthlyRecords;

            List<KeyValuePair<string, double>> spendingData = new List<KeyValuePair<string, double>>();
            foreach (IGrouping<string, double> period in spendingRecordsByMonth)
                spendingData.Add(new KeyValuePair<string, double>(period.Key, period.Sum()));

            ((LineSeries) ChartOverviewLeft.Series[1]).ItemsSource = spendingData;
        }

        private void LoadSpendingDistributionData()
        {
            List<Debit> spendingRecords = debitQuery.ToList();

            List<KeyValuePair<string, double>> spendingDistributionData = new List<KeyValuePair<string, double>>();
            foreach (string spendingCategory in Debit.DebitCategories)
            {
                double categoryTotal = (from record in spendingRecords
                    where record.Category == spendingCategory
                    select record.Amount).Sum();

                if (categoryTotal > 0)
                    spendingDistributionData.Add(new KeyValuePair<string, double>(spendingCategory, categoryTotal));
            }
            ((PieSeries) ChartOverviewRight.Series[0]).ItemsSource = spendingDistributionData;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Transaction newTransaction;
            if (RadioButtonDebit.IsChecked == true)
                newTransaction = new Debit(DatePickerAdd.DisplayDate.Date, ComboBoxAddPerson.Text,
                    ComboBoxAddCategory.Text,
                    double.Parse(TextBoxAddAmount.Text), TextBoxAddNote.Text);
            else
                newTransaction = new Credit(DatePickerAdd.DisplayDate, ComboBoxAddPerson.Text, ComboBoxAddCategory.Text,
                    double.Parse(TextBoxAddAmount.Text), TextBoxAddNote.Text);

            records.Add(newTransaction);
        }

        private void RadioButtonCredit_Checked(object sender, RoutedEventArgs e)
        {
            ComboBoxAddCategory.ItemsSource = Credit.CreditCategories;
            ComboBoxAddCategory.SelectedIndex = 0;
        }

        private void RadioButtonCredit_Unchecked(object sender, RoutedEventArgs e)
        {
            ComboBoxAddCategory.ItemsSource = Debit.DebitCategories;
            ComboBoxAddCategory.SelectedIndex = 0;
        }
    }
}