using System.Collections.Generic;

namespace Budgeteer_Web.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public bool IsDebit { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}