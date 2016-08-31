using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budgeteer.Web.Angular.Models
{
    public class Transactions
    {
        public int TransactionId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public string Note { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual AspNetUsers User { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Categories Category { get; set; }
    }
}