using System.ComponentModel.DataAnnotations;

namespace Budgeteer.Web.MVC.Models
{
    public class CategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        public bool IsDebit { get; set; }
    }
}