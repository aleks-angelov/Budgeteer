using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budgeteer_Web.Models
{
    public abstract class Transaction
    {
        public int TransactionID { get; set; }

        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }

        [ForeignKey("Person")]
        public int UserID { get; set; }
        
        public virtual ApplicationUser Person { get; set; }

        public int TypeID { get; set; }

        public virtual Type Type { get; set; }

        public int CategoryID { get; set; }

        public virtual Category Catergory { get; set; }
    }
}