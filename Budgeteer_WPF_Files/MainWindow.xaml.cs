using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace Budgeteer_WPF_Files
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static ObservableCollection<Transaction> _records = new ObservableCollection<Transaction>();

        private static IEnumerable<Debit> _debitQuery;

        private static IEnumerable<Credit> _creditQuery;

        private static readonly BinaryFormatter BinFormat = new BinaryFormatter();

        public MainWindow()
        {
            InitializeComponent();

            _records.CollectionChanged += RecordsOnCollectionChanged;
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

        private void HomeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataFromBinary();
            MessageBox.Show($"People: {Transaction.People.Count}\nRecords: {_records.Count}");
            SetupOverviewTab();
            SetupSpendingTab();
            SetupIncomeTab();
            //SetupSettingsTab();
        }

        private static void LoadDataFromBinary()
        {
            using (Stream fStream = File.OpenRead("People.bin"))
                Transaction.People = (ObservableCollection<string>) BinFormat.Deserialize(fStream);

            using (Stream fStream = File.OpenRead("DebitCategories.bin"))
                Debit.DebitCategories = (ObservableCollection<string>) BinFormat.Deserialize(fStream);

            using (Stream fStream = File.OpenRead("CreditCategories.bin"))
                Credit.CreditCategories = (ObservableCollection<string>) BinFormat.Deserialize(fStream);

            using (Stream fStream = File.OpenRead("Records.bin"))
                _records = (ObservableCollection<Transaction>) BinFormat.Deserialize(fStream);

            _debitQuery = from record in _records
                where record.Type == "Debit"
                select record as Debit;

            _creditQuery = from record in _records
                where record.Type == "Credit"
                select record as Credit;
        }

        private void HomeWindow_Closed(object sender, EventArgs e)
        {
            SaveDataToBinary();
        }

        private static void SaveDataToBinary()
        {
            using (Stream fStream = new FileStream("People.bin",
                FileMode.Create, FileAccess.Write, FileShare.None))
                BinFormat.Serialize(fStream, Transaction.People);

            using (Stream fStream = new FileStream("DebitCategories.bin",
                FileMode.Create, FileAccess.Write, FileShare.None))
                BinFormat.Serialize(fStream, Debit.DebitCategories);

            using (Stream fStream = new FileStream("CreditCategories.bin",
                FileMode.Create, FileAccess.Write, FileShare.None))
                BinFormat.Serialize(fStream, Credit.CreditCategories);

            using (Stream fStream = new FileStream("Records.bin",
                FileMode.Create, FileAccess.Write, FileShare.None))
                BinFormat.Serialize(fStream, _records);
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
            double amount;
            if (!double.TryParse(TextBoxAddAmount.Text, out amount))
            {
                MessageBox.Show("Please enter a valid number for the amount.", "Invalid Amount", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            Transaction newTransaction;
            if (RadioButtonDebit.IsChecked == true)
                newTransaction = new Debit(DatePickerAdd.DisplayDate, ComboBoxAddPerson.Text,
                    ComboBoxAddCategory.Text,
                    amount, TextBoxAddNote.Text);
            else
                newTransaction = new Credit(DatePickerAdd.DisplayDate, ComboBoxAddPerson.Text, ComboBoxAddCategory.Text,
                    amount, TextBoxAddNote.Text);

            _records.Add(newTransaction);
        }
    }
}