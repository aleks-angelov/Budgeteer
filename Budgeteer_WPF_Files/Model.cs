using System;
using System.Collections.ObjectModel;

namespace Budgeteer_WPF_Files
{
    internal abstract class Transaction
    {
        public static ObservableCollection<string> People = new ObservableCollection<string>
        {
            "Aleks Angelov",
            "Boris Ruskov",
            "Mariya Stancheva"
        };

        protected Transaction(DateTime d, string p, string t, string c, float a, string n = "")
        {
            Date = d;
            Person = p;
            Type = t;
            Category = c;
            Amount = a;
            Note = n;
        }

        public DateTime Date { get; set; }
        public string Person { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public float Amount { get; set; }
        public string Note { get; set; }
    }

    internal class Debit : Transaction
    {
        public static ObservableCollection<string> DebitCategories = new ObservableCollection<string>
        {
            "Groceries",
            "Personal Care",
            "Transportation",
            "Entertainment",
            "Restaurants",
            "Utilities",
            "Loans",
            "Rent",
            "Clothing",
            "Healthcare",
            "Travel",
            "Equipment",
            "Miscellaneous"
        };

        public Debit(DateTime d, string p, string c, float a, string n = "")
            : base(d, p, "Debit", c, a, n)
        {
        }
    }

    internal class Credit : Transaction
    {
        public static ObservableCollection<string> CreditCategories = new ObservableCollection<string>
        {
            "Salary",
            "Bonuses",
            "Dividends",
            "Other"
        };

        public Credit(DateTime d, string p, string c, float a, string n = "")
            : base(d, p, "Credit", c, a, n)
        {
        }
    }
}