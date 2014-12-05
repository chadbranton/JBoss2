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

        // Allow auditing creation and modification of models that implement IAuditedEntity
        // See: http://benjii.me/2014/03/track-created-and-modified-fields-automatically-with-entity-framework-code-first/
        // And: http://stackoverflow.com/a/26357308
        public override int SaveChanges()
        {
            var addedAuditedEntities = ChangeTracker.Entries<IAuditedEntity>()
              .Where(p => p.State == EntityState.Added)
              .Select(p => p.Entity);

            var modifiedAuditedEntities = ChangeTracker.Entries<IAuditedEntity>()
              .Where(p => p.State == EntityState.Modified)
              .Select(p => p.Entity);

            var now = DateTime.UtcNow;

            var currentUsername = HttpContext.Current != null && HttpContext.Current.User != null
            ? HttpContext.Current.User.Identity.Name
            : "Anonymous";

            foreach (var added in addedAuditedEntities)
            {
                added.CreatedAt = now;
                added.CreatedBy = currentUsername;
                added.LastModifiedAt = now;
                added.LastModifiedBy = currentUsername;
            }

            foreach (var modified in modifiedAuditedEntities)
            {
                modified.LastModifiedAt = now;
                modified.LastModifiedBy = currentUsername;
            }

            return base.SaveChanges();
        }

    }

    // Implementing this interface causes creation and modification information
    // to be modified appropriately on SaveChanges(). 
    // Note: There may be a better place to put this.
    public interface IAuditedEntity
    {
        string CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        string LastModifiedBy { get; set; }
        DateTime LastModifiedAt { get; set; }
    }
}