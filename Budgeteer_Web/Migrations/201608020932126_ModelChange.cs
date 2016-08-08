using System.Data.Entity.Migrations;

namespace Budgeteer_Web.Migrations
{
    public partial class ModelChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "IsDebit", c => c.Boolean(false));
            DropColumn("dbo.Transactions", "IsDebit");
        }

        public override void Down()
        {
            AddColumn("dbo.Transactions", "IsDebit", c => c.Boolean(false));
            DropColumn("dbo.Categories", "IsDebit");
        }
    }
}