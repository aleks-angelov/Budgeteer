using System;
using System.Collections.Generic;

namespace Budgeteer_Web_Angular.Models
{
    public partial class Transactions
    {
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }

        public virtual Categories Category { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
