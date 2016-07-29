using System;
using System.Collections.ObjectModel;

namespace Budgeteer_Web.Models
{
    public class CreditTransaction : Transaction
    {
        public static ObservableCollection<string> CreditCategories;

        public CreditTransaction(DateTime d, string p, string c, double a, string n = "")
            : base(d, p, "Credit", c, a, n)
        {
        }
    }
}