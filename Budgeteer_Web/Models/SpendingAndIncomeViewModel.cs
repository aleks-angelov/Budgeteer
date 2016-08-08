using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Budgeteer_Web.Models
{
    public class SpendingAndIncomeViewModel
    {
        public SpendingAndIncomeViewModel(bool isSpending, string userId)
        {
            using (ApplicationDbContext context = ApplicationDbContext.Create())
            {
                UserItems = new List<SelectListItem>();
                foreach (ApplicationUser user in context.Users.OrderBy(u => u.Name))
                    UserItems.Add(new SelectListItem { Text = user.Name, Value = user.Name });
                
                List<Category> categories = context.Categories.Where(c => c.IsDebit == isSpending).OrderBy(c => c.Name).ToList();
                ApplicationUser currentUser = context.Users.Single(u => u.Id == userId);

                CategoryItems = new List<SelectListItem>();
                foreach (Category cat in categories)
                {
                    if(currentUser.Categories.Contains(cat))
                        CategoryItems.Add(new SelectListItem { Text = cat.Name, Value = cat.Name });
                }
            }
        }

        [Required]
        public string PersonName { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateUntil { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string NewCategoryName { get; set; }

        public List<SelectListItem> UserItems { get; }
        public List<SelectListItem> CategoryItems { get; }
    }
}