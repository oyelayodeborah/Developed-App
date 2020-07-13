namespace KABE_Food_Ordering_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "VerificationCode", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "CardNumber", c => c.String(nullable: false, maxLength: 16));
            AddColumn("dbo.Restaurants", "SmallThumbnailjpg", c => c.String());
            AddColumn("dbo.Restaurants", "SmallThumbnailpng", c => c.String());
            AddColumn("dbo.Restaurants", "FoodId", c => c.String());
            AddColumn("dbo.Restaurants", "Price", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Restaurants", "SmallThumbnailpng");
            DropColumn("dbo.Restaurants", "SmallThumbnailjpg");
            DropColumn("dbo.Orders", "CardNumber");
            DropColumn("dbo.Orders", "VerificationCode");
            DropColumn("dbo.Restaurants", "Price");
            DropColumn("dbo.Restaurants", "FoodId");
        }
    }
}
