using System;
using System.Collections.Generic;
using System.Linq;
using Budgeteer.Web.Angular.Models;
using Microsoft.AspNetCore.Mvc;

namespace Budgeteer.Web.Angular.Controllers
{
    [Route("api/[controller]")]
    public class TransactionsController : Controller
    {
        private readonly BudgeteerDbContext _context;
        private const int PageSize = 10;

        public TransactionsController(BudgeteerDbContext context)
        {
            _context = context;
        }

        // GET: api/values
        [HttpGet]
        public int[] GetPageNumbers()
        {
            int totalPages = (int)Math.Ceiling((decimal)_context.Transactions.Count() / PageSize);
            int[] pageNumbers = new int[totalPages];
            for (int i = 0; i < totalPages; i++)
                pageNumbers[i] = i + 1;

            return pageNumbers;
        }

        [HttpGet("{page}")]
        public IEnumerable<TransactionViewModel> Get(int page)
        {
            List<Transactions> transactions = _context.Transactions.OrderByDescending(t => t.Date)
                .ThenBy(t => t.User.Name)
                .ThenBy(t => t.Category.Name)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return TransactionViewModel.Convert(transactions, _context);
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