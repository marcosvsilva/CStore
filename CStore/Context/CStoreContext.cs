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

        private DbSet<Order> Orders { get; set; }

        private DbSet<Item> Items { get; set; }

        private DbSet<Payment> Payments { get; set; }

        private DbSet<Product> Products { get; set; }

        private DbSet<Brand> Brands { get; set; }

        private DbSet<Category> Categories { get; set; }

        private DbSet<User> Users { get; set; }
    }
}

