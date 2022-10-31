using System;
using CStore.Models;
using Microsoft.EntityFrameworkCore;

namespace CStore.Context
{
    public class CStoreContext : DbContext
    {
        public CStoreContext(DbContextOptions options) : base(options)
        {
            //pass;
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }
    }
}

