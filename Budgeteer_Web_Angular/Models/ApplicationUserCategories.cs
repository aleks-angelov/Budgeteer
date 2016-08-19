using System;
using System.Collections.Generic;

namespace Budgeteer_Web_Angular.Models
{
    public partial class ApplicationUserCategories
    {
        public string ApplicationUserId { get; set; }
        public int CategoryCategoryId { get; set; }

        public virtual AspNetUsers ApplicationUser { get; set; }
        public virtual Categories CategoryCategory { get; set; }
    }
}
