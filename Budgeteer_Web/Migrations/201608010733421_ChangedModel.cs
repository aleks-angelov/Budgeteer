using System.Data.Entity.Migrations;

namespace Budgeteer_Web.Migrations
{
    public partial class ChangedModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "TransTypeID", "dbo.TransTypes");
            DropForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Categories", new[] {"TransTypeID"});
            DropIndex("dbo.Transactions", new[] {"CategoryID"});
            AddColumn("dbo.Transactions", "Category", c => c.String());
            DropColumn("dbo.Transactions", "CategoryID");
            DropTable("dbo.Categories");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                {
                    CategoryID = c.Int(false, true),
                    Name = c.String(),
                    TransTypeID = c.Int()
                })
                .PrimaryKey(t => t.CategoryID);

            AddColumn("dbo.Transactions", "CategoryID", c => c.Int());
            DropColumn("dbo.Transactions", "Category");
            CreateIndex("dbo.Transactions", "CategoryID");
            CreateIndex("dbo.Categories", "TransTypeID");
            AddForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories", "CategoryID");
            AddForeignKey("dbo.Categories", "TransTypeID", "dbo.TransTypes", "TransTypeID");
        }
    }
}