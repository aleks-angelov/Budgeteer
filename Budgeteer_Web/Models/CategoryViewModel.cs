using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Budgeteer_Web.Models
{
    public class CategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        public bool IsDebit { get; set; }
    }
}