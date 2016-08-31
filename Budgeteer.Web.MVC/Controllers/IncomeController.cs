using System.Web.Mvc;
using Budgeteer.Web.MVC.Models;

namespace Budgeteer.Web.MVC.Controllers
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