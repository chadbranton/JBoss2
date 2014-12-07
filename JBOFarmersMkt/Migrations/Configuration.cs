namespace JBOFarmersMkt.Migrations
{
    using System;
    using JBOFarmersMkt.Context;
    using JBOFarmersMkt.Models;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using System.Web.Mvc;
    using WebMatrix.WebData;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<JBOFarmersMkt.Context.JBOContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(JBOFarmersMkt.Context.JBOContext context)
        {
            //  This method will be called after migrating to the latest version.
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: false);
            }

            // Initialize roles
            List<string> roles = new List<string> { "Administrator", "Developer", "Supplier", "Member" };
            foreach (var role in roles)
            {
                if (!Roles.RoleExists(role))
                {
                    Roles.CreateRole(role);
                }
            }

            // Initialize Users. This should be updated in production.
            // Passwords should definitely not be "password".
            // Consider: http://www.asp.net/identity/overview/features-api/best-practices-for-deploying-passwords-and-other-sensitive-data-to-aspnet-and-azure
            var users = new Dictionary<string, List<string>>() 
            {
                {"admin", new List<string>{"Administrator"}},
                {"ryan", new List<string>{"Administrator", "Developer"}},
                {"chad", new List<string>{"Supplier"}},
                {"joe", new List<string>{"Supplier"}},
                {"tom", new List<string>{"Member"}}
            };

            foreach (var user in users)
            {
                if (!WebSecurity.UserExists(user.Key))
                {
                    WebSecurity.CreateUserAndAccount(user.Key, "password");
                }

                foreach (var role in user.Value)
                {
                    if (!Roles.GetRolesForUser(user.Key).Contains(role))
                    {
                        Roles.AddUserToRole(user.Key, role);
                    }
                }
            }

            context.SaveChanges();

            // Create some suppliers
            var joe = context.UserProfiles.First(u => u.UserName == "joe");
            var chad = context.UserProfiles.First(u => u.UserName == "chad");

            Supplier chadsVegetables = new Supplier
                {
                    name = "Chad's Vegetables",
                    address = "1234 abc lane",
                    city = "Jonesborough",
                    state = "TN",
                    email = "chad@example.com",
                    phone = "4235551234"
                };

            Supplier joesApples = new Supplier
                    {
                        name = "Joe's Apples",
                        address = "4567 Caramel Valley Road",
                        city = "Morristown",
                        state = "TN",
                        email = "joesapples@example.com",
                        phone = "4235551235"
                    };

            context.Suppliers.AddOrUpdate(s => s.name, chadsVegetables, joesApples);

            context.SaveChanges();

            // refresh
            chadsVegetables = context.Suppliers.Find(chadsVegetables.supplierID);
            joesApples = context.Suppliers.Find(joesApples.supplierID);

            // Assign users to suppliers
            if (!chadsVegetables.users.Contains(chad))
            {
                chadsVegetables.users.Add(chad);
            }

            if (!joesApples.users.Contains(joe))
            {
                joesApples.users.Add(joe);
            }

            // Save changes
            context.SaveChanges();

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

            base.Seed(context);
        }
    }
}
