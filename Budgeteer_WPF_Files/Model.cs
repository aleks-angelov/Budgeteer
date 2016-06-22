using System;
using System.Collections.Generic;

namespace Budgeteer_WPF_Files
{
    internal abstract class Transaction
    {
        public static List<string> People = new List<string> {"Aleks Angelov", "Boris Ruskov, Mariya Stancheva"};

        protected DateTime Date;
        protected string Person;
        protected string Category;
        protected float Amount;
        protected string Note;

        protected Transaction(DateTime d, string p, string c, float a, string n = "")
        {
            Date = d;
            Person = p;
            Category = c;
            Amount = a;
            Note = n;
        }
    }

    internal class Debit : Transaction
    {
        public static List<string> DebitCategories = new List<string>
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
            : base(d, p, c, a, n)
        {
        }
    }

    internal class Credit : Transaction
    {
        public static List<string> CreditCategories = new List<string> {"Salary", "Bonuses", "Dividends", "Other"};

        public Credit(DateTime d, string p, string c, float a, string n = "")
            : base(d, p, c, a, n)
        {
        }
    }
}
