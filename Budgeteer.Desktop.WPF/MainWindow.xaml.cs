using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;

namespace Budgeteer.Desktop.WPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static ObservableCollection<Transaction> _records;

        private static IEnumerable<Debit> _debitQuery;
        private static IEnumerable<Credit> _creditQuery;

        private static readonly XmlSerializer StringSerializer = new XmlSerializer(typeof(ObservableCollection<string>));

        private static readonly XmlSerializer TransactionSerializer =
            new XmlSerializer(typeof(ObservableCollection<Transaction>));

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
            using (Stream fStream = new FileStream("People.xml", FileMode.OpenOrCreate))
            {
                if (fStream.Length > 0)
                    Transaction.People = (ObservableCollection<string>) StringSerializer.Deserialize(fStream);
                else
                    Transaction.People = new ObservableCollection<string>();
            }

            using (Stream fStream = new FileStream("DebitCategories.xml", FileMode.OpenOrCreate))
            {
                if (fStream.Length > 0)
                    Debit.DebitCategories = (ObservableCollection<string>) StringSerializer.Deserialize(fStream);
                else
                    Debit.DebitCategories = new ObservableCollection<string>();
            }

            using (Stream fStream = new FileStream("CreditCategories.xml", FileMode.OpenOrCreate))
            {
                if (fStream.Length > 0)
                    Credit.CreditCategories = (ObservableCollection<string>) StringSerializer.Deserialize(fStream);
                else
                    Credit.CreditCategories = new ObservableCollection<string>();
            }

            using (Stream fStream = new FileStream("Records.xml", FileMode.OpenOrCreate))
            {
                if (fStream.Length > 0)
                    _records = (ObservableCollection<Transaction>) TransactionSerializer.Deserialize(fStream);
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
            using (Stream fStream = new FileStream("People.xml", FileMode.Create))
            {
                StringSerializer.Serialize(fStream, Transaction.People);
            }

            using (Stream fStream = new FileStream("DebitCategories.xml", FileMode.Create))
            {
                StringSerializer.Serialize(fStream, Debit.DebitCategories);
            }

            using (Stream fStream = new FileStream("CreditCategories.xml", FileMode.Create))
            {
                StringSerializer.Serialize(fStream, Credit.CreditCategories);
            }

            using (Stream fStream = new FileStream("Records.xml", FileMode.Create))
            {
                TransactionSerializer.Serialize(fStream, _records);
            }
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

            var person = ComboBoxAddPerson.Text;
            if (!Transaction.People.Contains(person))
            {
                var wasInserted = false;
                var pos = 0;
                while (pos < Transaction.People.Count)
                {
                    if (string.Compare(person, Transaction.People[pos], StringComparison.OrdinalIgnoreCase) <= 0)
                    {
                        Transaction.People.Insert(pos, person);
                        wasInserted = true;
                        break;
                    }
                    pos++;
                }

                if (!wasInserted)
                    Transaction.People.Add(person);
            }

            Transaction newTransaction;
            if (RadioButtonDebit.IsChecked == true)
            {
                var debitCategory = ComboBoxAddCategory.Text;
                if (!Debit.DebitCategories.Contains(debitCategory))
                {
                    var wasInserted = false;
                    var pos = 0;
                    while (pos < Debit.DebitCategories.Count)
                    {
                        if (
                            string.Compare(debitCategory, Debit.DebitCategories[pos], StringComparison.OrdinalIgnoreCase) <=
                            0)
                        {
                            Debit.DebitCategories.Insert(pos, debitCategory);
                            wasInserted = true;
                            break;
                        }
                        pos++;
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
                var creditCategory = ComboBoxAddCategory.Text;
                if (!Credit.CreditCategories.Contains(creditCategory))
                {
                    var wasInserted = false;
                    var pos = 0;
                    while (pos < Credit.CreditCategories.Count)
                    {
                        if (string.Compare(creditCategory, Credit.CreditCategories[pos],
                                StringComparison.OrdinalIgnoreCase) <= 0)
                        {
                            Credit.CreditCategories.Insert(pos, creditCategory);
                            wasInserted = true;
                            break;
                        }
                        pos++;
                    }

                    if (!wasInserted)
                        Credit.CreditCategories.Add(creditCategory);
                }

                newTransaction = new Credit(DatePickerAdd.SelectedDate.Value, person, creditCategory,
                    amount, TextBoxAddNote.Text);
            }
            _records.Add(newTransaction);
            TextBoxAddAmount.Clear();
            TextBoxAddNote.Clear();
        }
    }
}