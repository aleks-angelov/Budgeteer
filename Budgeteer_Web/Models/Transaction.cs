using System;
using System.Collections.ObjectModel;

namespace Budgeteer_Web.Models
{
    public abstract class Transaction
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

        public int TransactionID { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }

        public string Person { get; set; }


        public string Type { get; set; }


        public string Category { get; set; }


    }
}