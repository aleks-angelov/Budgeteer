using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Budgeteer.Desktop.WPF
{
    [Serializable]
    [XmlInclude(typeof(Debit))]
    [XmlInclude(typeof(Credit))]
    public abstract class Transaction
    {
        public static ObservableCollection<string> People;

        protected Transaction()
        {
        }

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
    public class Debit : Transaction
    {
        public static ObservableCollection<string> DebitCategories;

        public Debit()
        {
        }

        public Debit(DateTime d, string p, string c, double a, string n = "")
            : base(d, p, "Debit", c, a, n)
        {
        }
    }

    [Serializable]
    public class Credit : Transaction
    {
        public static ObservableCollection<string> CreditCategories;

        public Credit()
        {
        }

        public Credit(DateTime d, string p, string c, double a, string n = "")
            : base(d, p, "Credit", c, a, n)
        {
        }
    }
}