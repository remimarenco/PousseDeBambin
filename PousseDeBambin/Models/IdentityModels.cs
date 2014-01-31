using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PousseDeBambin.Migrations;

namespace PousseDeBambin.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public int Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public virtual ICollection<List> Lists { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public DbSet<Prospect> Prospects { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<GiftState> GiftsStates { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}