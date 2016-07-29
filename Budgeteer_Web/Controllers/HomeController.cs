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
            //ApplicationDbContext ctx = new ApplicationDbContext();

            //TransType t = new TransType() {Name="debit"};
            //ctx.TransTypes.Add(t);
            //ctx.SaveChanges();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}