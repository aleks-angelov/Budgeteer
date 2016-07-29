using System;
using System.Collections.ObjectModel;

namespace Budgeteer_Web.Models
{
    public class DebitTransaction : Transaction
    {
        public static ObservableCollection<string> DebitCategories;

        public DebitTransaction(DateTime d, string p, string c, double a, string n = "") 
            : base(d, p, "Debit", c, a, n)
        {
        }
    }
}