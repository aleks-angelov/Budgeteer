using System.Collections.Generic;
using System.Linq;
using Budgeteer.Web.Angular.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budgeteer.Web.Angular.Controllers
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