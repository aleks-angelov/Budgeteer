using System.Data.Entity.Migrations;

namespace Budgeteer_Web.Migrations
{
    public partial class TransactionAddRest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "TransTypeID", c => c.Int(false));
            AddColumn("dbo.Transactions", "CategoryID", c => c.Int());
            CreateIndex("dbo.Transactions", "TransTypeID");
            CreateIndex("dbo.Transactions", "CategoryID");
            AddForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories", "CategoryID");
            AddForeignKey("dbo.Transactions", "TransTypeID", "dbo.TransTypes", "TransTypeID", true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "TransTypeID", "dbo.TransTypes");
            DropForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Transactions", new[] {"CategoryID"});
            DropIndex("dbo.Transactions", new[] {"TransTypeID"});
            DropColumn("dbo.Transactions", "CategoryID");
            DropColumn("dbo.Transactions", "TransTypeID");
        }
    }
}