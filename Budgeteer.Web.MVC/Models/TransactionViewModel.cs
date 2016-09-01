using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Budgeteer.Web.MVC.Models
{
    public class TransactionViewModel
    {
        public TransactionViewModel()
        {
            Date = DateTime.Today;

            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                List<string> userNames = context.Users.Select(usr => usr.Name).OrderBy(name => name).ToList();
                Users = new List<SelectListItem>();
                foreach (string userName in userNames)
                    Users.Add(new SelectListItem { Text = userName, Value = userName });
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