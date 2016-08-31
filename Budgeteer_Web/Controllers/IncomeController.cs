using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Budgeteer_Web.Models;
using Microsoft.AspNet.Identity;

namespace Budgeteer_Web.Controllers
{
    public class IncomeController : Controller
    {
        // GET: Income
        [Authorize]
        public ActionResult Index()
        {
            return View(new SpendingAndIncomeViewModel(false));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(SpendingAndIncomeViewModel ivm)
        {
            return View(ivm);
        }

        [Authorize]
        public ActionResult AddIncomeCategory()
        {
            CategoryViewModel cvm = new CategoryViewModel
            {
                IsDebit = false
            };

            return PartialView("AddCategory", cvm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddIncomeCategory(CategoryViewModel cvm)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            Category existingCategory =
                context.Categories.FirstOrDefault(c => c.Name.Equals(cvm.Name, StringComparison.OrdinalIgnoreCase));
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = context.Users.Single(u => u.Id == userId);

            if (existingCategory != null)
            {
                if (!currentUser.Categories.Contains(existingCategory))
                    existingCategory.ApplicationUsers.Add(currentUser);
            }
            else
            {
                Category newCategory = new Category
                {
                    Name = cvm.Name,
                    IsDebit = cvm.IsDebit,
                    ApplicationUsers = new List<ApplicationUser> { currentUser }
                };
                context.Categories.Add(newCategory);
            }

            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}