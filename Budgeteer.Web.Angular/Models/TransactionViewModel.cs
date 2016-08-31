using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Budgeteer.Web.Angular.Models
{
    public class TransactionViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public string Note { get; set; }

        [Required]
        public string PersonName { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public bool IsDebit { get; set; }

        //Transactions to TransactionViewModel converter
        public static IEnumerable<TransactionViewModel> Convert(IEnumerable<Transactions> transactions,
            BudgeteerDbContext context)
        {
            List<TransactionViewModel> transactionViewModels = new List<TransactionViewModel>();

            foreach (Transactions tr in transactions)
            {
                Categories trCat = context.Categories.Single(cat => cat.CategoryId == tr.CategoryId);

                transactionViewModels.Add(new TransactionViewModel
                {
                    Date = tr.Date,
                    Amount = tr.Amount,
                    Note = tr.Note,
                    PersonName = context.AspNetUsers.Single(usr => usr.Id == tr.UserId).Name,
                    CategoryName = trCat.Name,
                    IsDebit = trCat.IsDebit
                });
            }

            return transactionViewModels;
        }
    }
}