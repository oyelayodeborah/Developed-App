namespace KABE_Food_Ordering_System.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using KABE_Food_Ordering_System.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<KABE_Food_Ordering_System.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(KABE_Food_Ordering_System.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            var getRole = context.Roles.ToList().Count();
            if (getRole == 0)
            {
                context.Roles.AddOrUpdate(x => x.Id, new Role() { Name = "Admin" });
            }
        }
    }
}
