using System;
using System.Collections.ObjectModel;

namespace Budgeteer.Desktop.WPF
{
    [Serializable]
    internal abstract class Transaction
    {
        public static ObservableCollection<string> People;

        protected Transaction(DateTime d, string p, string t, string c, double a, string n = "")
        {
            Date = d;
            Person = p;
            Type = t;
            Category = c;
            Amount = Math.Round(a, 2);
            Note = n;
        }

        public DateTime Date { get; set; }
        public string Person { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
    }

    [Serializable]
    internal class Debit : Transaction
    {
        public static ObservableCollection<string> DebitCategories;

        public Debit(DateTime d, string p, string c, double a, string n = "")
            : base(d, p, "Debit", c, a, n)
        {
        }
    }

    [Serializable]
    internal class Credit : Transaction
    {
        public static ObservableCollection<string> CreditCategories;

        public Credit(DateTime d, string p, string c, double a, string n = "")
            : base(d, p, "Credit", c, a, n)
        {
        }
    }
}