namespace Budgeteer_Web.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }

        public int? TypeID { get; set; }
        public virtual Type Type { get; set; }
    }
}