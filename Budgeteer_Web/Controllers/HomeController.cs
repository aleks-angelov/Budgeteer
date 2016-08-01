using System;
using System.Linq;
using System.Web.Mvc;
using Budgeteer_Web.Models;

namespace Budgeteer_Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ApplicationDbContext _context = ApplicationDbContext.Create();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Overview()
        {
            return View(_context.Transactions.Where(t => t.Person.Email == "aia131@aubg.edu").ToList());
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
            return PartialView();
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
                Person = _context.Users.First(u => u.Name == personName),
                Type = _context.TransTypes.First(t => t.Name == typeName),
                Category = _context.Categories.First(c => c.Name == categoryName)
            };
            
            _context.Transactions.Add(newTransaction);
            _context.SaveChanges();

            return RedirectToAction("Overview");
        }
    }
}