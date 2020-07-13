namespace KABE_Food_Ordering_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb5 : DbMigration
    {
       
        public override void Up()
        {
            AlterColumn("dbo.Orders", "CardNumber", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "ResidentialAddress", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "TransactionReference", c => c.String(nullable: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Orders", "CardNumber", c => c.String(nullable: false, maxLength: 16));
            AlterColumn("dbo.Orders", "TransactionReference", c => c.String(nullable: false, maxLength: 40));
            DropColumn("dbo.Orders", "ResidentialAddress");
        }
    }
}
