using PousseDeBambin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PousseDeBambin.DAL
{
    public class PdbDbContext : DbContext
    {
        public PdbDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<PdbDbContext>(null);
        }

        public DbSet<Prospect> Prospects { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<GiftState> GiftsStates { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<ApplicationUser> UserProfiles { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}