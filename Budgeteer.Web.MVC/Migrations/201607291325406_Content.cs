using System.Data.Entity.Migrations;

namespace Budgeteer.Web.MVC.Migrations
{
    public partial class Content : DbMigration
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
                    "dbo.TransTypes",
                    c => new
                    {
                        TransTypeID = c.Int(false, true),
                        Name = c.String()
                    })
                .PrimaryKey(t => t.TransTypeID);

            CreateTable(
                    "dbo.Transactions",
                    c => new
                    {
                        TransactionID = c.Int(false, true),
                        Date = c.DateTime(false),
                        Amount = c.Double(false),
                        Note = c.String()
                    })
                .PrimaryKey(t => t.TransactionID);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Categories", "TransTypeID", "dbo.TransTypes");
            DropIndex("dbo.Categories", new[] {"TransTypeID"});
            DropTable("dbo.Transactions");
            DropTable("dbo.TransTypes");
            DropTable("dbo.Categories");
        }
    }
}