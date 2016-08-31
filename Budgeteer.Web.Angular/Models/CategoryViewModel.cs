using System.ComponentModel.DataAnnotations;

namespace Budgeteer.Web.Angular.Models
{
    public class CategoryViewModel
    {
        [Required]
        public string Name { get; set; }

        public bool IsDebit { get; set; }
    }
}