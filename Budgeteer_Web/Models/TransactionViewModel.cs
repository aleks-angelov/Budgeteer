using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

            Categories = new List<SelectListItem>();
            foreach (Category cat in context.Categories)
                Categories.Add(new SelectListItem { Text = cat.Name, Value = cat.Name });
        }

        public int TransactionID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public string Note { get; set; }

        public bool IsDebit { get; set; }

        [Required]
        public string PersonName { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public List<SelectListItem> Users { get; }
        public List<SelectListItem> Categories { get; }
    }
}