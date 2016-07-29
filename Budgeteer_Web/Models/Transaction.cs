using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Budgeteer_Web.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }

        [Required]
        public DateTime Date { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Note { get; set; }

        [ForeignKey("Person")]
        public string UserID { get; set; }

        public virtual ApplicationUser Person { get; set; }

        public int TransTypeID { get; set; }

        public virtual TransType Type { get; set; }

        public int? CategoryID { get; set; }

        public virtual Category Catergory { get; set; }
    }
}