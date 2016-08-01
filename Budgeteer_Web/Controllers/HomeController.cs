using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            ApplicationDbContext ctx = ApplicationDbContext.Create();

            return View(ctx.Transactions.Where(t => t.Person.Email == "aia131@aubg.edu").ToList());
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
        public ActionResult AddTransaction(TransactionViewModel trans)
        {


            return PartialView(trans);
        }
    }
}