using System.Data.Entity.Migrations;

namespace Budgeteer.Web.MVC.Migrations
{
    public partial class TransactionAddUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Transactions", "UserID");
            AddForeignKey("dbo.Transactions", "UserID", "dbo.AspNetUsers", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Transactions", new[] {"UserID"});
            DropColumn("dbo.Transactions", "UserID");
        }
    }
}