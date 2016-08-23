using System.Collections.Generic;
using System.Linq;
using Budgeteer_Web_Angular.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budgeteer_Web_Angular.Controllers
{
    [Route("api/[controller]")]
    public class TransactionsController : Controller
    {
        private readonly BudgeteerDbContext _context;

        public TransactionsController(BudgeteerDbContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<TransactionViewModel> Get()
        {
            List<Transactions> transactions = _context.Transactions.OrderByDescending(t => t.Date)
                .ThenBy(t => t.User.Name)
                .ThenBy(t => t.Category.Name)
                .Take(10)
                .ToList();

            List<TransactionViewModel> transactionViewModels = new List<TransactionViewModel>();
            foreach (Transactions tr in transactions)
            {
                Categories trCat = _context.Categories.Single(cat => cat.CategoryId == tr.CategoryId);

                transactionViewModels.Add(new TransactionViewModel
                {
                    Date = tr.Date,
                    Amount = tr.Amount,
                    Note = tr.Note,
                    PersonName = _context.AspNetUsers.Single(usr => usr.Id == tr.UserId).Name,
                    CategoryName = trCat.Name,
                    IsDebit = trCat.IsDebit
                });
            }

            return transactionViewModels;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] TransactionViewModel tvm)
        {
            Transactions tr = new Transactions
            {
                Date = tvm.Date.Date,
                Amount = tvm.Amount,
                Note = tvm.Note,
                User = _context.AspNetUsers.First(u => u.Name == tvm.PersonName),
                Category = _context.Categories.First(c => c.Name == tvm.CategoryName)
            };
            _context.Transactions.Add(tr);
            _context.SaveChanges();
        }
    }
}