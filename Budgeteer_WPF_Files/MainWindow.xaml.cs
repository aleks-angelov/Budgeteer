using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Budgeteer_WPF_Files
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<Transaction> records = new ObservableCollection<Transaction>
        {
            new Debit(DateTime.Today, "Aleks Angelov", "Groceries", 10.05f, "Lunch and dinner")
        };

        public MainWindow()
        {
            InitializeComponent();

            SetupOverviewTab();
        }

        private void SetupOverviewTab()
        {
            ComboBoxAddPerson.ItemsSource = Transaction.People;
            ComboBoxAddCategory.ItemsSource = Debit.DebitCategories;

            DataGridAdd.ItemsSource = records;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            Transaction newTransaction;
            if (RadioButtonDebit.IsChecked == true)
                newTransaction = new Debit(DatePickerAdd.DisplayDate.Date, ComboBoxAddPerson.Text,
                    ComboBoxAddCategory.Text,
                    float.Parse(TextBoxAddAmount.Text), TextBoxAddNote.Text);
            else
                newTransaction = new Credit(DatePickerAdd.DisplayDate, ComboBoxAddPerson.Text, ComboBoxAddCategory.Text,
                    float.Parse(TextBoxAddAmount.Text), TextBoxAddNote.Text);

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