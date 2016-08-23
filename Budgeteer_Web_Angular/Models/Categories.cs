using System.Collections.Generic;

namespace Budgeteer_Web_Angular.Models
{
    public class Categories
    {
        public Categories()
        {
            ApplicationUserCategories = new HashSet<ApplicationUserCategories>();
            Transactions = new HashSet<Transactions>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsDebit { get; set; }

        public virtual ICollection<ApplicationUserCategories> ApplicationUserCategories { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}