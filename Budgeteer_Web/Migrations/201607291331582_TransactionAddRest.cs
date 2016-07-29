namespace Budgeteer_Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransactionAddRest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "TransTypeID", c => c.Int(nullable: false));
            AddColumn("dbo.Transactions", "CategoryID", c => c.Int());
            CreateIndex("dbo.Transactions", "TransTypeID");
            CreateIndex("dbo.Transactions", "CategoryID");
            AddForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories", "CategoryID");
            AddForeignKey("dbo.Transactions", "TransTypeID", "dbo.TransTypes", "TransTypeID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "TransTypeID", "dbo.TransTypes");
            DropForeignKey("dbo.Transactions", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Transactions", new[] { "CategoryID" });
            DropIndex("dbo.Transactions", new[] { "TransTypeID" });
            DropColumn("dbo.Transactions", "CategoryID");
            DropColumn("dbo.Transactions", "TransTypeID");
        }
    }
}
