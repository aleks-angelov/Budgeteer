using System;
using System.Linq;
using System.Web.Mvc;
using Budgeteer_Web.Models;

namespace Budgeteer_Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ApplicationDbContext Context = ApplicationDbContext.Create();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Overview()
        {
            return View(Context.Transactions.ToList());
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
        [HttpPost]
        public ActionResult AddTransaction(TransactionViewModel tvm)
        {
            Transaction newTransaction = new Transaction
            {
                Date = tvm.Date,
                Amount = tvm.Amount,
                Note = tvm.Note,
                Person = Context.Users.First(u => u.Name == tvm.PersonName),
                Category = Context.Categories.First(c => c.Name == tvm.CategoryName)
            };
            
            Context.Transactions.Add(newTransaction);
            Context.SaveChanges();

            return RedirectToRoute(new {controller="Home", action="Overview"});
        }
    }
}