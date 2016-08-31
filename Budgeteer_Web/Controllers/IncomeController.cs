using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Budgeteer_Web.Models;
using Microsoft.AspNet.Identity;

namespace Budgeteer_Web.Controllers
{
    public class IncomeController : Controller
    {
        // GET: Income
        [Authorize]
        public ActionResult Index()
        {
            return View(new SpendingAndIncomeViewModel(false));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(SpendingAndIncomeViewModel ivm)
        {
            return View(ivm);
        }
    }
}