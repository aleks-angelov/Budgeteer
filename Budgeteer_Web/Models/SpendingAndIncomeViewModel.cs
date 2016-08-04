using System;
using System.ComponentModel.DataAnnotations;

namespace Budgeteer_Web.Models
{
    public class SpendingAndIncomeViewModel
    {
        [Required]
        public string PersonName { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateUntil { get; set; }

        [Required]
        public string CategoryName { get; set; }
    }
}