using System.Collections.Generic;
using System.Linq;
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
        [HttpGet("{name}")]
        public IEnumerable<TransactionViewModel> Get(string name)
        {
            return TransactionViewModel.Convert(_context.Transactions.ToList(), _context);
        }
    }
}
