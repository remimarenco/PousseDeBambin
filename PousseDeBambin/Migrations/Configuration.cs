using PousseDeBambin.Models;

namespace PousseDeBambin.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PousseDeBambin.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "PousseDeBambin.Models.ApplicationDbContext";
        }

        protected override void Seed(PousseDeBambin.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            
            context.Users.AddOrUpdate(
                p => p.UserName,
                new ApplicationUser
                {
                    UserName = "Anonyme",
                    FirstName = "Ano",
                    LastName = "Nyme",
                    EmailAddress = "remi@mercilacigogne.com",
                    PhoneNumber = "0614914252",
                    Street = "40 avenue guy de collongue",
                    Zipcode = 69130,
                    City = "Ecully",
                    Country = "France"
                }
            );
        }
    }
}
