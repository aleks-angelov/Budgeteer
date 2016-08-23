using System.Collections.Generic;
using System.Linq;
using Budgeteer_Web_Angular.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budgeteer_Web_Angular.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : Controller
    {
        private readonly BudgeteerDbContext _context;

        public CategoriesController(BudgeteerDbContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get(bool debit)
        {
            List<Categories> categories =
                _context.Categories.Where(c => c.IsDebit == debit).OrderBy(c => c.Name).ToList();
            IEnumerable<string> categoryNames = categories.Select(c => c.Name);

            return categoryNames;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] CategoryViewModel cvm)
        {
            Categories cat = new Categories
            {
                Name = cvm.Name,
                IsDebit = cvm.IsDebit
            };
            _context.Categories.Add(cat);
            _context.SaveChanges();
        }
    }
}