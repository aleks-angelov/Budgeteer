using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace Budgeteer
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static ObservableCollection<Transaction> _records;

        private static IEnumerable<Debit> _debitQuery;
        private static IEnumerable<Credit> _creditQuery;

        private static readonly BinaryFormatter BinFormat = new BinaryFormatter();

        public MainWindow()
        {
            InitializeComponent();
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
            _records.CollectionChanged += RecordsOnCollectionChanged;
            SetupOverviewTab();
            SetupSpendingTab();
            SetupIncomeTab();
        }

        private static void LoadDataFromBinary()
        {
            using (Stream fStream = new FileStream("People.bin",
                FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                if (fStream.Length > 0)
                    Transaction.People = (ObservableCollection<string>) BinFormat.Deserialize(fStream);
                else
                    Transaction.People = new ObservableCollection<string>();
            }

            using (Stream fStream = new FileStream("DebitCategories.bin",
                FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                if (fStream.Length > 0)
                    Debit.DebitCategories = (ObservableCollection<string>) BinFormat.Deserialize(fStream);
                else
                    Debit.DebitCategories = new ObservableCollection<string>();
            }

            using (Stream fStream = new FileStream("CreditCategories.bin",
                FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                if (fStream.Length > 0)
                    Credit.CreditCategories = (ObservableCollection<string>) BinFormat.Deserialize(fStream);
                else
                    Credit.CreditCategories = new ObservableCollection<string>();
            }

            using (Stream fStream = new FileStream("Records.bin",
                FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                if (fStream.Length > 0)
                    _records = (ObservableCollection<Transaction>) BinFormat.Deserialize(fStream);
                else
                    _records = new ObservableCollection<Transaction>();
            }

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

            string person = ComboBoxAddPerson.Text;
            if (!Transaction.People.Contains(person))
            {
                bool wasInserted = false;
                for (int i = 0; i < Transaction.People.Count; i++)
                {
                    if (string.Compare(person, Transaction.People[i], StringComparison.OrdinalIgnoreCase) <= 0)
                    {
                        Transaction.People.Insert(i, person);
                        wasInserted = true;
                        break;
                    }
                }

                if (!wasInserted)
                    Transaction.People.Add(person);
            }

            Transaction newTransaction;
            if (RadioButtonDebit.IsChecked == true)
            {
                string debitCategory = ComboBoxAddCategory.Text;
                if (!Debit.DebitCategories.Contains(debitCategory))
                {
                    bool wasInserted = false;
                    for (int i = 0; i < Debit.DebitCategories.Count; i++)
                    {
                        if (
                            string.Compare(debitCategory, Debit.DebitCategories[i], StringComparison.OrdinalIgnoreCase) <=
                            0)
                        {
                            Debit.DebitCategories.Insert(i, debitCategory);
                            wasInserted = true;
                            break;
                        }
                    }

                    if (!wasInserted)
                        Debit.DebitCategories.Add(debitCategory);
                }

                newTransaction = new Debit(DatePickerAdd.SelectedDate.Value, person,
                    debitCategory,
                    amount, TextBoxAddNote.Text);
            }
            else
            {
                string creditCategory = ComboBoxAddCategory.Text;
                if (!Credit.CreditCategories.Contains(creditCategory))
                {
                    bool wasInserted = false;
                    for (int i = 0; i < Credit.CreditCategories.Count; i++)
                    {
                        if (string.Compare(creditCategory, Credit.CreditCategories[i],
                            StringComparison.OrdinalIgnoreCase) <= 0)
                        {
                            Credit.CreditCategories.Insert(i, creditCategory);
                            wasInserted = true;
                            break;
                        }
                    }

                    if (!wasInserted)
                        Credit.CreditCategories.Add(creditCategory);
                }

                newTransaction = new Credit(DatePickerAdd.SelectedDate.Value, person, creditCategory,
                    amount, TextBoxAddNote.Text);
            }
            _records.Add(newTransaction);
        }
    }
}