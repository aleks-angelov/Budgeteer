using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Budgeteer.Web.MVC.Models;

namespace Budgeteer.Web.MVC.Controllers
{
    public class OverviewController : Controller
    {
        private const int PageSize = 10;

        // GET: Overview
        [Authorize]
        public ActionResult Index(int page = 1)
        {
            ApplicationDbContext context = ApplicationDbContext.Create();

            return View(new TransactionPagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = context.Transactions.Count()
            });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Index(TransactionViewModel tvm)
        {
            ApplicationDbContext context = ApplicationDbContext.Create();
            Transaction newTransaction = new Transaction
            {
                Date = tvm.Date,
                Amount = tvm.Amount,
                Note = tvm.Note,
                Person = context.Users.First(u => u.Name == tvm.PersonName),
                Category = context.Categories.First(c => c.Name == tvm.CategoryName)
            };

            context.Transactions.Add(newTransaction);
            context.SaveChanges();

            return RedirectToAction("ListTransactions");
        }

        [Authorize]
        public JsonResult GetCategoryNames(bool debit)
        {
            ApplicationDbContext context = ApplicationDbContext.Create();
            List<Category> categories = context.Categories.Where(c => c.IsDebit == debit).OrderBy(c => c.Name).ToList();
            IEnumerable<string> data = categories.Select(c => c.Name);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ListTransactions(int page = 1)
        {
            ApplicationDbContext context = ApplicationDbContext.Create();

            return
                PartialView(context.Transactions.OrderByDescending(t => t.Date)
                    .ThenBy(t => t.Person.Name)
                    .ThenBy(t => t.Category.Name)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList());
        }
    }
}