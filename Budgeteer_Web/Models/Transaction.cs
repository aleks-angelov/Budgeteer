using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budgeteer_Web.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public double Amount { get; set; }
        
        public string Note { get; set; }

        [ForeignKey("Person")]
        public string UserID { get; set; }

        public virtual ApplicationUser Person { get; set; }

        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
    }
}