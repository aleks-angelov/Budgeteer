using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Budgeteer_Web.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }

        public int? TransTypeID { get; set; }
        public virtual TransType TransType { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}