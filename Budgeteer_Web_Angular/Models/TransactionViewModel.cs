using System;
using System.ComponentModel.DataAnnotations;

namespace Budgeteer_Web_Angular.Models
{
    public class TransactionViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public string Note { get; set; }

        [Required]
        public string PersonName { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public bool IsDebit { get; set; }
    }
}