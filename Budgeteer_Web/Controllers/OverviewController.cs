using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Budgeteer_Web.Models;
using Microsoft.AspNet.Identity;

namespace Budgeteer_Web.Controllers
{
    public class OverviewController : Controller
    {
        // GET: Overview
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(TransactionViewModel tvm)
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
        public JsonResult GetCategoryNames(bool debit)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            List<Category> categories = context.Categories.Where(c => c.IsDebit == debit).OrderBy(c => c.Name).ToList();
            string userId = User.Identity.GetUserId();
            ApplicationUser currentUser = context.Users.Single(u => u.Id == userId);

            var data = categories.Where(c => currentUser.Categories.Contains(c)).Select(c => new { catName = c.Name });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [ChildActionOnly]
        public ActionResult AddTransaction()
        {
            return PartialView(new TransactionViewModel(User.Identity.GetUserId()));
        }

        [Authorize]
        public ActionResult ListTransactions()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            return
                PartialView(context.Transactions.OrderByDescending(t => t.Date)
                    .ThenBy(t => t.Person.Name)
                    .ThenBy(t => t.Category.Name)
                    .Take(10)
                    .ToList());
        }
    }
}