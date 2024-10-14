using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MShopStyle.Areas.Identity;
using Microsoft.EntityFrameworkCore;
using MShopStyle.Areas.Identity.Data;
using MShopStyle.Models;
using Microsoft.AspNetCore.Identity;



namespace MShopStyle.Models
{
    public class MShopStyleContext : IdentityDbContext<MShopStyleUser> 
    {
 
        public MShopStyleContext(DbContextOptions<MShopStyleContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }

        public DbSet<Artist> Artist { get; set; }

        public DbSet<Genre> Genre { get; set; }

        public DbSet<ArtistGenre> ArtistGenre { get; set; }

        public DbSet<CheckoutModel> CheckOut { get; set; }
        public DbSet<Review> Review { get; set; }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}

