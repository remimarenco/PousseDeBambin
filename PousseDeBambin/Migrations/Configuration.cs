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
            
            /* Cr�ation des utilisateurs de base */
            context.Users.AddOrUpdate(
                p => p.UserName,
                new ApplicationUser
                {
                    UserName = "Anonyme",
                    FirstName = "Ano",
                    LastName = "Nyme",
                    EmailAddress = "remi@poussedebambin.com",
                    PhoneNumber = "0614914252",
                    Street = "40 avenue guy de collongue",
                    Zipcode = 69130,
                    City = "Ecully",
                    Country = "France"
                },
                new ApplicationUser
                {
                    UserName = "Maioune",
                    FirstName = "Marion",
                    LastName = "Wang",
                    EmailAddress = "marion@poussedebambin.com",
                    PhoneNumber = "0630799036",
                    Street = "15 rue Jules Verne",
                    Zipcode = 69003,
                    City = "Lyon",
                    Country = "France"
                },
                new ApplicationUser
                {
                    UserName = "remimarenco",
                    FirstName = "R�mi",
                    LastName = "Marenco",
                    EmailAddress = "remi@poussedebambin.com",
                    PhoneNumber = "0614914252",
                    Street = "141 rue antoine charial",
                    Zipcode = 69003,
                    City = "Lyon",
                    Country = "France"
                },
                new ApplicationUser
                {
                    UserName = "Bertrand",
                    FirstName = "Bertrand",
                    LastName = "Deher",
                    EmailAddress = "deher.bertrand@gmail.com",
                    PhoneNumber = "0614914252",
                    Street = "141 rue antoine charial",
                    Zipcode = 69003,
                    City = "Lyon",
                    Country = "France"
                }
            );

            // Ajout des listes

            context.SaveChanges();
        }
    }
}
