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
        public ActionResult DisplayChart(string chartName, string br, DateTime dateFrom, DateTime dateUntil,
            string personName = null, string categoryName = null)
        {
            Chart chart = ChartFactory.CreateChart(chartName, dateFrom, dateUntil, personName, categoryName);

            return File(chart.GetBytes(), "image/png");
        }
    }
}