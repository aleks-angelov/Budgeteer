using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Budgeteer_Web.Infrastructure;
using Budgeteer_Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Budgeteer_Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Overview()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Overview(TransactionViewModel tvm)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            Transaction newTransaction = new Transaction
            {
                Date = tvm.Date,
                Amount = tvm.Amount,
                Note = tvm.Note,
                Person = context.Users.First(u => u.Name == tvm.PersonName),
                Category = context.Categories.First(c => c.Name == tvm.CategoryName)
            };

            context.Transactions.Add(newTransaction);
            context.SaveChanges();

            return RedirectToAction("ListTransactions");
        }

        [Authorize]
        public ActionResult Spending()
        {
            return View(new SpendingAndIncomeViewModel(true, User.Identity.GetUserId()));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Spending(SpendingAndIncomeViewModel svm)
        {
            return View(svm);
        }

        [Authorize]
        public ActionResult Income()
        {
            return View(new SpendingAndIncomeViewModel(false, User.Identity.GetUserId()));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Income(SpendingAndIncomeViewModel ivm)
        {
            return View(ivm);
        }

        [Authorize]
        [ChildActionOnly]
        public ActionResult AddTransaction()
        {
            return PartialView(new TransactionViewModel());
        }

        [Authorize]
        public JsonResult GetCategoryNames(bool debit)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            ApplicationUser currentUser = context.Users.Single(u => u.Id == User.Identity.GetUserId());

            var data =
                context.Categories.Where(c => currentUser.Categories.Contains(c) && c.IsDebit == debit)
                    .OrderBy(c => c.Name)
                    .Select(c => new { catName = c.Name });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ListTransactions()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            return
                PartialView(
                    context.Transactions.OrderByDescending(t => t.Date)
                        .ThenBy(t => t.Person.Name)
                        .ThenBy(t => t.Category.Name)
                        .ToList());
        }

        [Authorize]
        public ActionResult DisplayChart(string chartName, string br, DateTime dateFrom, DateTime dateUntil,
            string personName = null, string categoryName = null)
        {
            Chart chart = ChartFactory.CreateChart(chartName, dateFrom, dateUntil, personName, categoryName);

            return File(chart.GetBytes(), "image/png");
        }

        [Authorize]
        public ActionResult AddCategory(bool isDebit)
        {
            CategoryViewModel cvm = new CategoryViewModel
            {
                IsDebit = isDebit
            };

            return PartialView(cvm);
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

            if (existingCategory != null && !currentUser.Categories.Contains(existingCategory))
                existingCategory.ApplicationUsers.Add(currentUser);

            else
            {
                Category newCategory = new Category
                {
                    Name = cvm.Name,
                    IsDebit = cvm.IsDebit,
                    ApplicationUsers = new List<ApplicationUser>
                    {
                        context.Users.First(u => u.Email == currentUser.Email)
                    }
                };
                context.Categories.Add(newCategory);
            }

            context.SaveChanges();

            return RedirectToAction(cvm.IsDebit ? "Spending" : "Income");
        }
    }
}