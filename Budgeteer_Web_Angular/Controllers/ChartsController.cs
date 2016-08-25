using System;
using System.Collections.Generic;
using Budgeteer_Web_Angular.Infrastructure;
using Budgeteer_Web_Angular.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budgeteer_Web_Angular.Controllers
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
        public IEnumerable<TransactionViewModel> Get(string chartName, DateTime dateFrom, DateTime dateUntil,
            string personName = null, string categoryName = null)
        {
            //IEnumerable<Transactions> chartTransactions = ChartDataFactory.GetChartTransactions(chartName, dateFrom, dateUntil, personName, categoryName, _context);
            return null;
            //return TransactionViewModel.Convert(chartTransactions, _context);
        }
    }
}
