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

            //WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: false);

            if (!Roles.RoleExists("Administrator"))
            {
                Roles.CreateRole("Administrator");
            }

            if (!WebSecurity.UserExists("admin"))
            {
                WebSecurity.CreateUserAndAccount("admin", "password");
            }

            if (!Roles.GetRolesForUser("admin").Contains("Administrator"))
            {
                Roles.AddUserToRole("admin", "Administrator");
            }
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
