using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Budgeteer.Web.MVC.Models;

namespace Budgeteer.Web.MVC.Controllers
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
            ApplicationDbContext context = ApplicationDbContext.Create();
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
            ApplicationDbContext context = ApplicationDbContext.Create();
            List<Category> categories = context.Categories.Where(c => c.IsDebit == debit).OrderBy(c => c.Name).ToList();
            IEnumerable<string> data = categories.Select(c => c.Name);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ListTransactions()
        {
            ApplicationDbContext context = ApplicationDbContext.Create();

            return
                PartialView(context.Transactions.OrderByDescending(t => t.Date)
                    .ThenBy(t => t.Person.Name)
                    .ThenBy(t => t.Category.Name)
                    .Take(10)
                    .ToList());
        }
    }
}