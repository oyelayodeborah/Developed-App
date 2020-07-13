using System;
using System.Web;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KABE_Food_Ordering_System.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Restaurant>().Property(b => b.Foods).HasConversion(v => JsonConvert.SerializeObject(v),v => JsonConvert.DeserializeObject<Dictionary<string, string>>(v));

            //modelBuilder.Entity<Restaurant>().Property(e => e.Foods).GetType();
            //modelBuilder.Entity<Restaurant>().Property

            //    (u => u.Foods)
            //        .HasConversion(
            //            d => JsonConvert.SerializeObject(d, Formatting.None),
            //            s => JsonConvert.DeserializeObject<Dictionary<string, object>>(s)
            //        )
            //        .HasMaxLength(4000)
            //        .IsRequired();
            }



        public DbSet<Role> Roles { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Food> Foods { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }




        //protected override void Seed(ApplicationDbContext context)
        //{
        //    //This method will be called after migrating to the latest version.
        //    //  You can use the DbSet<T>.AddOrUpdate() helper extension method
        //    //  to avoid creating duplicate seed data.
        //    //var getBranch = context.Branches.ToList().Count();
        //    //if (getBranch == 0)
        //    //{
        //    //    context.Branches.AddOrUpdate(x => x.id, new Branch() { name = "Yaba" });

        //    //}
        //    var getRole = context.Roles.ToList().Count();

        //    if (getRole == 0)
        //    {
        //        context.Roles.AddOrUpdate(x => x.Id, new Role() { Name = "Admin" });
        //    }
        //}

    }
}

