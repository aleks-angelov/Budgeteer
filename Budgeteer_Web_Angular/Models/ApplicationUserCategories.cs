namespace Budgeteer_Web_Angular.Models
{
    public class ApplicationUserCategories
    {
        public string ApplicationUserId { get; set; }
        public int CategoryCategoryId { get; set; }

        public virtual AspNetUsers ApplicationUser { get; set; }
        public virtual Categories CategoryCategory { get; set; }
    }
}