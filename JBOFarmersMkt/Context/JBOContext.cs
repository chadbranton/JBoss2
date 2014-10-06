using JBOFarmersMkt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JBOFarmersMkt.Context
{
    public class JBOContext : DbContext
    {
        public JBOContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Import> Imports { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Return> Returns { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Membership> Memberships { get; set; }
      

    }
}