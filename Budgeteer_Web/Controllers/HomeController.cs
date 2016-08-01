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
            return View(Context.Transactions.Where(t => t.Person.Email == "aia131@aubg.edu").ToList());
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
        [ChildActionOnly]
        [HttpPost]
        public ActionResult AddTransaction(FormCollection formCol)
        {
            string personName = formCol["person"];
            string typeName = formCol["transtype"];
            string categoryName = formCol["category"];

            Transaction newTransaction = new Transaction
            {
                Date = DateTime.Parse(formCol["date"]),
                Amount = double.Parse(formCol["amount"]),
                Note = formCol["note"],
                Person = Context.Users.First(u => u.Name == personName),
                Type = Context.TransTypes.First(t => t.Name == typeName),
                Category = Context.Categories.First(c => c.Name == categoryName)
            };
            
            Context.Transactions.Add(newTransaction);
            Context.SaveChanges();

            return View("Overview");
        }
    }
}