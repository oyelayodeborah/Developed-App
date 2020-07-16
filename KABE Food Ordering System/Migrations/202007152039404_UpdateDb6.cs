namespace KABE_Food_Ordering_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "ResidentialAddress", c => c.String(nullable: true));
            AlterColumn("dbo.Orders", "TransactionReference", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "TransactionReference", c => c.String(nullable: false, maxLength: 40));
            DropColumn("dbo.Orders", "ResidentialAddress");
        }
    }
}
