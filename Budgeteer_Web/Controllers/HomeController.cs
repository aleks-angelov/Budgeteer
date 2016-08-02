using System.Linq;
using System.Web.Mvc;
using Budgeteer_Web.Models;

namespace Budgeteer_Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ApplicationDbContext Context = ApplicationDbContext.Create();

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Overview()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Overview(TransactionViewModel tvm)
        {
            Transaction newTransaction = new Transaction
            {
                Date = tvm.Date,
                Amount = tvm.Amount,
                Note = tvm.Note,
                Person = Context.Users.First(u => u.Name == tvm.PersonName),
                Category = Context.Categories.First(c => c.Name == tvm.CategoryName)
            };

            Context.Transactions.Add(newTransaction);
            Context.SaveChanges();

            return View();
        }

        [Authorize]
        public ActionResult Spending()
        {
            return View();
        }

        [Authorize]
        public ActionResult Income()
        {
            return View();
        }

        [Authorize]
        public ActionResult AddTransaction(bool debit = true)
        {
            if (Request.IsAjaxRequest())
            {
                var data =
                    Context.Categories.Where(c => c.IsDebit == debit).Select(c => new
                    {
                        catName = c.Name
                    });

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return PartialView(new TransactionViewModel());
        }

        [Authorize]
        [ChildActionOnly]
        public ActionResult ListTransactions()
        {
            return PartialView(Context.Transactions.ToList());
        }
    }
}