using System;
using Budgeteer.Web.Angular.Infrastructure;
using Budgeteer.Web.Angular.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budgeteer.Web.Angular.Controllers
{
    [Route("api/[controller]")]
    public class ChartsController : Controller
    {
        private readonly BudgeteerDbContext _context;

        public ChartsController(BudgeteerDbContext context)
        {
            _context = context;
        }

        // GET api/values/5
        [HttpGet]
        public ChartData Get(string chartName, DateTime dateFrom, DateTime dateUntil,
            string personName = null, string categoryName = null)
        {
            return ChartDataFactory.GetChartData(_context, chartName, dateFrom, dateUntil, personName, categoryName);
        }
    }
}