using System.Collections.Generic;
using System.Linq;
using Budgeteer_Web_Angular.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budgeteer_Web_Angular.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly BudgeteerDbContext _context;

        public UsersController(BudgeteerDbContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<string> userNames = _context.AspNetUsers.Select(usr => usr.Name).OrderBy(name => name).ToList();

            return userNames;
        }
    }
}
