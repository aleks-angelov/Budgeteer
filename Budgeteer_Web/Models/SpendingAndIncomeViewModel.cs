using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Budgeteer_Web.Models
{
    public class SpendingAndIncomeViewModel
    {
        public SpendingAndIncomeViewModel(bool isSpending)
        {
            DateUntil = DateTime.Today;
            DateFrom = DateUntil.AddMonths(-6);

            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                UserItems = new List<SelectListItem>();
                foreach (ApplicationUser user in context.Users.OrderBy(u => u.Name))
                    UserItems.Add(new SelectListItem {Text = user.Name, Value = user.Name});

                CategoryItems = new List<SelectListItem>();
                foreach (Category cat in context.Categories.Where(c => c.IsDebit == isSpending).OrderBy(c => c.Name))
                    CategoryItems.Add(new SelectListItem {Text = cat.Name, Value = cat.Name});
            }
        }

        [Required]
        public string PersonName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateUntil { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string NewCategoryName { get; set; }

        public List<SelectListItem> UserItems { get; }
        public List<SelectListItem> CategoryItems { get; }
    }
}