using System;
using System.ComponentModel.DataAnnotations;

namespace Budgeteer_Web.Models
{
    public class TransactionViewModel
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string PersonName { get; set; }

        public bool IsDebit { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public double Amount { get; set; }

        public string Note { get; set; }
    }
}