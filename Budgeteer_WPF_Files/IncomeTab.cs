using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgeteer_WPF_Files
{
    public partial class MainWindow
    {
        private void SetupIncomeTab()
        {
            //ComboBoxSpendingPerson.ItemsSource = Transaction.People;
            //DatePickerSpendingFrom.SelectedDate = DateTime.Today.AddMonths(-6);
            //DatePickerSpendingFrom.SelectedDateChanged += FilterChanged;
            //DatePickerSpendingUntil.SelectedDate = DateTime.Today;
            //DatePickerSpendingUntil.SelectedDateChanged += FilterChanged;
            //ComboBoxSpendingCategory.ItemsSource = Debit.DebitCategories;

            ReloadIncomeData();
        }

        private void ReloadIncomeData()
        {
            throw new NotImplementedException();
        }
    }
}
