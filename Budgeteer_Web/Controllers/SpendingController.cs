using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Budgeteer_Web.Models;
using Microsoft.AspNet.Identity;

namespace Budgeteer_Web.Controllers
{
    public class SpendingController : Controller
    {
        // GET: Spending
        [Authorize]
        public ActionResult Index()
        {
            return View(new SpendingAndIncomeViewModel(true));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(SpendingAndIncomeViewModel svm)
        {
            return View(svm);
        }
    }
}