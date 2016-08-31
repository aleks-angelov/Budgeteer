using System.Data.Entity.Migrations;

namespace Budgeteer.Web.MVC.Migrations
{
    public partial class ImprovedModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Categories",
                    c => new
                    {
                        CategoryID = c.Int(false, true),
                        Name = c.String(),
                        TransTypeID = c.Int()
                    })
                .PrimaryKey(t => t.CategoryID)
                .ForeignKey("dbo.TransTypes", t => t.TransTypeID)
                .Index(t => t.TransTypeID);

            CreateTable(
                    "dbo.ApplicationUserCategories",
                    c => new
                    {
                        ApplicationUser_Id = c.String(false, 128),
                        Category_CategoryID = c.Int(false)
                    })
                .PrimaryKey(t => new {t.ApplicationUser_Id, t.Category_CategoryID})
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, true)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryID, true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Category_CategoryID);

            AddColumn("dbo.Transactions", "CategoryID", c => c.Int());
            CreateIndex("dbo.Transactions", "CategoryID");
            AddForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories", "CategoryID");
            DropColumn("dbo.Transactions", "Category");
        }

        public override void Down()
        {
            AddColumn("dbo.Transactions", "Category", c => c.String());
            DropForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Categories", "TransTypeID", "dbo.TransTypes");
            DropForeignKey("dbo.ApplicationUserCategories", "Category_CategoryID", "dbo.Categories");
            DropForeignKey("dbo.ApplicationUserCategories", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserCategories", new[] {"Category_CategoryID"});
            DropIndex("dbo.ApplicationUserCategories", new[] {"ApplicationUser_Id"});
            DropIndex("dbo.Transactions", new[] {"CategoryID"});
            DropIndex("dbo.Categories", new[] {"TransTypeID"});
            DropColumn("dbo.Transactions", "CategoryID");
            DropTable("dbo.ApplicationUserCategories");
            DropTable("dbo.Categories");
        }
    }
}