using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Budgeteer_Web.Models
{
    public class TransactionViewModel
    {
        public TransactionViewModel()
        {
        }

        public TransactionViewModel(string userId)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                ApplicationUser currentUser = context.Users.Single(u => u.Id == userId);
                Users = new List<SelectListItem>
                {
                    new SelectListItem {Text = currentUser.Name, Value = currentUser.Name}
                };
            }
        }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public string Note { get; set; }

        [Required]
        public string PersonName { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public List<SelectListItem> Users { get; }
    }
}