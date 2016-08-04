using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Budgeteer_Web.Infrastructure;
using Budgeteer_Web.Models;

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
            return View();
        }

        [Authorize]
        public ActionResult Income()
        {
            return View();
        }

        [Authorize]
        [ChildActionOnly]
        public ActionResult AddTransaction()
        {
            return PartialView(new TransactionViewModel());
        }

        [Authorize]
        public JsonResult GetCategoryNames(bool debit = true)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var data = context.Categories.Where(c => c.IsDebit == debit).Select(c => new { catName = c.Name });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ListTransactions()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            return PartialView(context.Transactions.ToList());
        }

        [Authorize]
        public ActionResult DisplayChart(string name, string br)
        {
            Chart chart = ChartFactory.CreateChart(name);

            return File(chart.GetBytes(), "image/png");
        }
    }
}