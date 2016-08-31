using System.Data.Entity.Migrations;

namespace Budgeteer.Web.MVC.Migrations
{
    public partial class RemovedTransType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "TransTypeID", "dbo.TransTypes");
            DropForeignKey("dbo.Transactions", "TransTypeID", "dbo.TransTypes");
            DropForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Categories", new[] {"TransTypeID"});
            DropIndex("dbo.Transactions", new[] {"TransTypeID"});
            DropIndex("dbo.Transactions", new[] {"CategoryID"});
            AddColumn("dbo.Transactions", "IsDebit", c => c.Boolean(false));
            AlterColumn("dbo.Transactions", "CategoryID", c => c.Int(false));
            CreateIndex("dbo.Transactions", "CategoryID");
            AddForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories", "CategoryID", true);
            DropColumn("dbo.Categories", "TransTypeID");
            DropColumn("dbo.Transactions", "TransTypeID");
            DropTable("dbo.TransTypes");
        }

        public override void Down()
        {
            CreateTable(
                    "dbo.TransTypes",
                    c => new
                    {
                        TransTypeID = c.Int(false, true),
                        Name = c.String()
                    })
                .PrimaryKey(t => t.TransTypeID);

            AddColumn("dbo.Transactions", "TransTypeID", c => c.Int(false));
            AddColumn("dbo.Categories", "TransTypeID", c => c.Int());
            DropForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Transactions", new[] {"CategoryID"});
            AlterColumn("dbo.Transactions", "CategoryID", c => c.Int());
            DropColumn("dbo.Transactions", "IsDebit");
            CreateIndex("dbo.Transactions", "CategoryID");
            CreateIndex("dbo.Transactions", "TransTypeID");
            CreateIndex("dbo.Categories", "TransTypeID");
            AddForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories", "CategoryID");
            AddForeignKey("dbo.Transactions", "TransTypeID", "dbo.TransTypes", "TransTypeID", true);
            AddForeignKey("dbo.Categories", "TransTypeID", "dbo.TransTypes", "TransTypeID");
        }
    }
}