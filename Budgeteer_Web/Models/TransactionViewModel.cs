using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Budgeteer_Web.Models
{
    public class TransactionViewModel
    {
        public TransactionViewModel()
        {
            ApplicationDbContext context = ApplicationDbContext.Create();

            Users = new List<SelectListItem>();
            foreach (ApplicationUser user in context.Users)
                Users.Add(new SelectListItem { Text = user.Name, Value = user.Name });
        }

        public int TransactionID { get; set; }

        [Required]
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