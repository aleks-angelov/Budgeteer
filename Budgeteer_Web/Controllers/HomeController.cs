using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Budgeteer_Web.Infrastructure;
using Budgeteer_Web.Models;
using Microsoft.AspNet.Identity;

namespace Budgeteer_Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult DisplayChart(string chartName, string br, DateTime dateFrom, DateTime dateUntil,
            string personName = null, string categoryName = null)
        {
            Chart chart = ChartFactory.CreateChart(chartName, dateFrom, dateUntil, personName, categoryName);

            return File(chart.GetBytes(), "image/png");
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddCategory(CategoryViewModel cvm)
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

            return RedirectToAction("Index", cvm.IsDebit ? "Spending" : "Income");
        }
    }
}