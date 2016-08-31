using System.Web.Mvc;
using Budgeteer.Web.MVC.Models;

namespace Budgeteer.Web.MVC.Controllers
{
    public class SpendingController : Controller
    {
        // GET: Spending
        [Authorize]
        public ActionResult Index()
        {
            return View(new SpendingAndIncomeViewModel(true));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(SpendingAndIncomeViewModel svm)
        {
            return View(svm);
        }
    }
}