using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Budgeteer_Web.Infrastructure;
using Budgeteer_Web.Models;
using Microsoft.AspNet.Identity;

namespace Budgeteer_Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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
        public ActionResult DisplayChart(string chartName, string br, DateTime dateFrom, DateTime dateUntil,
            string personName = null, string categoryName = null)
        {
            Chart chart = ChartFactory.CreateChart(chartName, dateFrom, dateUntil, personName, categoryName);

            return File(chart.GetBytes(), "image/png");
        }
    }
}