using System;
using System.Collections.Generic;

namespace Budgeteer_WPF_Files
{
    internal abstract class Transaction
    {
        protected DateTime Date;
        protected string Person;
        protected float Amount;
        protected string Note;
    }

    internal class Debit : Transaction
    {
        public enum Categories
        {
            Food = 0,
            Rent = 1
        }
    }

    internal class Credit : Transaction
    {
        public enum Categories
        {
            Salary = 0,
            Bonus = 1
        }
    }
}
