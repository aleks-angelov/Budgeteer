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
            new Debit(DateTime.Today, "Aleks Angelov", "Groceries", 10.05, "Lunch and dinner")
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
            LoadSpendingDistributionData();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadBudgetBalanceData();
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
        }

        private void LoadSpendingData()
        {
            List<Debit> spendingRecords = debitQuery.ToList();

            List<KeyValuePair<string, double>> spendingData = new List<KeyValuePair<string, double>>();
            foreach (string spendingCategory in Debit.DebitCategories)
            {
                double categoryTotal = (from record in spendingRecords
                    where record.Category == spendingCategory
                    select record.Amount).Sum();

                if (categoryTotal > 0)
                    spendingData.Add(new KeyValuePair<string, double>(spendingCategory, categoryTotal));
            }
            ((PieSeries) ChartOverviewRight.Series[0]).ItemsSource = spendingData;
        }

        private void LoadSpendingDistributionData()
        {
            List<Debit> spendingRecords = debitQuery.ToList();

            List<KeyValuePair<string, double>> spendingData = new List<KeyValuePair<string, double>>();
            foreach (string spendingCategory in Debit.DebitCategories)
            {
                double categoryTotal = (from record in spendingRecords
                    where record.Category == spendingCategory
                    select record.Amount).Sum();

                if (categoryTotal > 0)
                    spendingData.Add(new KeyValuePair<string, double>(spendingCategory, categoryTotal));
            }
            ((PieSeries) ChartOverviewRight.Series[0]).ItemsSource = spendingData;
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