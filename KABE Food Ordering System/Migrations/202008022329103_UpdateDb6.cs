namespace KABE_Food_Ordering_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "RecommendedFoodOne", c => c.String());
            AddColumn("dbo.Customers", "RecommendedFoodTwo", c => c.String());
            AddColumn("dbo.Customers", "RecommendedFoodThree", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "RecommendedFoodThree");
            DropColumn("dbo.Customers", "RecommendedFoodTwo");
            DropColumn("dbo.Customers", "RecommendedFoodOne");
        }
    }
}
