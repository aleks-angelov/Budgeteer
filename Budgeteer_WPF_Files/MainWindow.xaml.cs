using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace Budgeteer_WPF_Files
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ObservableCollection<Transaction> records = new ObservableCollection<Transaction>
        {
            new Debit(DateTime.Today.AddMonths(-12), "Aleks Angelov", "Groceries", 100.05, "A"),
            new Debit(DateTime.Today.AddMonths(-11), "Boris Ruskov", "Personal Care", 50.05, "B"),
            new Debit(DateTime.Today.AddMonths(-10), "Mariya Stancheva", "Transportation", 40.05, "C"),
            new Debit(DateTime.Today.AddMonths(-9), "Aleks Angelov", "Entertainment", 10.05, "D"),
            new Debit(DateTime.Today.AddMonths(-8), "Boris Ruskov", "Groceries", 20.05, "E"),
            new Debit(DateTime.Today.AddMonths(-7), "Mariya Stancheva", "Personal Care", 30.05, "F"),
            new Debit(DateTime.Today.AddMonths(-6), "Aleks Angelov", "Transportation", 160.05, "G"),
            new Debit(DateTime.Today.AddMonths(-5), "Boris Ruskov", "Entertainment", 150.05, "H"),
            new Debit(DateTime.Today.AddMonths(-4), "Mariya Stancheva", "Groceries", 140.05, "I"),
            new Debit(DateTime.Today.AddMonths(-3), "Aleks Angelov", "Personal Care", 110.05, "J"),
            new Debit(DateTime.Today.AddMonths(-2), "Boris Ruskov", "Transportation", 120.05, "K"),
            new Debit(DateTime.Today.AddMonths(-1), "Mariya Stancheva", "Entertainment", 130.05, "L"),
            new Debit(DateTime.Today, "Aleks Angelov", "Groceries", 60.05),
            new Credit(DateTime.Today.AddMonths(-12), "Boris Ruskov", "Salary", 109.05),
            new Credit(DateTime.Today.AddMonths(-11), "Mariya Stancheva", "Bonuses", 48.05, "a"),
            new Credit(DateTime.Today.AddMonths(-10), "Aleks Angelov", "Dividends", 37.05, "b"),
            new Credit(DateTime.Today.AddMonths(-9), "Boris Ruskov", "Other", 16.05, "c"),
            new Credit(DateTime.Today.AddMonths(-8), "Mariya Stancheva", "Salary", 15.05, "d"),
            new Credit(DateTime.Today.AddMonths(-7), "Aleks Angelov", "Bonuses", 24.05, "e"),
            new Credit(DateTime.Today.AddMonths(-6), "Boris Ruskov", "Dividends", 164.05, "f"),
            new Credit(DateTime.Today.AddMonths(-5), "Mariya Stancheva", "Other", 145.05, "g"),
            new Credit(DateTime.Today.AddMonths(-4), "Aleks Angelov", "Salary", 136.05, "h"),
            new Credit(DateTime.Today.AddMonths(-3), "Boris Ruskov", "Bonuses", 117.05, "i"),
            new Credit(DateTime.Today.AddMonths(-2), "Mariya Stancheva", "Dividends", 118.05, "j"),
            new Credit(DateTime.Today.AddMonths(-1), "Aleks Angelov", "Other", 129.05, "k"),
            new Credit(DateTime.Today, "Boris Ruskov", "Salary", 60.05, "l")
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
        }

        private void RecordsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            ReloadData();
        }


        private void ReloadData()
        {
            ReloadOverviewData();
            ReloadSpendingData();
            ReloadIncomeData();
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetupOverviewTab();
            SetupSpendingTab();
            //SetupIncomeTab();
            //SetupSettingsTab();
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

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Transaction newTransaction;
            if (RadioButtonDebit.IsChecked == true)
                newTransaction = new Debit(DatePickerAdd.DisplayDate, ComboBoxAddPerson.Text,
                    ComboBoxAddCategory.Text,
                    double.Parse(TextBoxAddAmount.Text), TextBoxAddNote.Text);
            else
                newTransaction = new Credit(DatePickerAdd.DisplayDate, ComboBoxAddPerson.Text, ComboBoxAddCategory.Text,
                    double.Parse(TextBoxAddAmount.Text), TextBoxAddNote.Text);

            records.Add(newTransaction);
        }
    }
}