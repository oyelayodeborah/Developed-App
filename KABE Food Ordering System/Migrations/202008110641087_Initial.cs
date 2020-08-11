namespace KABE_Food_Ordering_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsersId = c.Int(nullable: false),
                        Name = c.String(maxLength: 225),
                        DateOfBirth = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 225),
                        Gender = c.String(),
                        BestFood = c.String(maxLength: 65),
                        ResidentialAddress = c.String(maxLength: 60),
                        LocationId = c.Int(nullable: false),
                        PhoneNumber = c.String(nullable: false, maxLength: 11),
                        FoodAllergies = c.String(),
                        RecommendedFood = c.String(),
                        RecommendedFoodOne = c.String(),
                        RecommendedFoodTwo = c.String(),
                        RecommendedFoodThree = c.String(),
                        Occupation = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UsersId, cascadeDelete: false)
                .Index(t => t.UsersId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 225),
                        Email = c.String(nullable: false, maxLength: 225),
                        SecretQuestions = c.String(maxLength: 225),
                        RoleId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        LastLoggedIn = c.DateTime(nullable: false),
                        LastLoggedOut = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: false)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Foods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Image = c.String(),
                        ContentType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        RestaurantId = c.Int(nullable: false),
                        Amount = c.String(nullable: false),
                        ResidentialAddress = c.String(nullable: false),
                        VerificationCode = c.String(nullable: false),
                        FoodQuantity = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                        TransactionReference = c.String(nullable: false),
                        CardNumber = c.String(nullable: false),
                        FoodId = c.Int(nullable: false),
                        OrderDateTime = c.DateTime(nullable: false),
                        DeliveryDateTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId, cascadeDelete: false)
                .ForeignKey("dbo.Foods", t => t.FoodId, cascadeDelete: false)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.RestaurantId, cascadeDelete: false)
                .Index(t => t.CustomerId)
                .Index(t => t.RestaurantId)
                .Index(t => t.LocationId)
                .Index(t => t.FoodId);
            
            CreateTable(
                "dbo.Restaurants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RestaurantsId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        ResidentialAddress = c.String(maxLength: 60),
                        LocationId = c.Int(nullable: false),
                        PhoneNumber = c.String(maxLength: 11),
                        Email = c.String(nullable: false, maxLength: 225),
                        EstablishmentDate = c.DateTime(nullable: false),
                        About = c.String(),
                        SmallThumbnailjpg = c.String(),
                        SmallThumbnailpng = c.String(),
                        FoodId = c.String(),
                        Price = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.RestaurantsId, cascadeDelete: false)
                .Index(t => t.RestaurantsId)
                .Index(t => t.LocationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Restaurants", "RestaurantsId", "dbo.Users");
            DropForeignKey("dbo.Restaurants", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Orders", "RestaurantId", "dbo.Users");
            DropForeignKey("dbo.Orders", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Orders", "FoodId", "dbo.Foods");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.Customers", "UsersId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Customers", "LocationId", "dbo.Locations");
            DropIndex("dbo.Restaurants", new[] { "LocationId" });
            DropIndex("dbo.Restaurants", new[] { "RestaurantsId" });
            DropIndex("dbo.Orders", new[] { "FoodId" });
            DropIndex("dbo.Orders", new[] { "LocationId" });
            DropIndex("dbo.Orders", new[] { "RestaurantId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Customers", new[] { "LocationId" });
            DropIndex("dbo.Customers", new[] { "UsersId" });
            DropTable("dbo.Restaurants");
            DropTable("dbo.Orders");
            DropTable("dbo.Foods");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Locations");
            DropTable("dbo.Customers");
        }
    }
}
