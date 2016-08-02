using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Budgeteer_Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Budgeteer_Web.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
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

            // Users

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(u => u.Email == "aia131@aubg.edu"))
            {
                ApplicationUser seedUser1 = new ApplicationUser
                {
                    Name = "Aleks Angelov",
                    Email = "aia131@aubg.edu",
                    UserName = "aia131@aubg.edu"
                };
                userManager.Create(seedUser1, "Password1");
            }

            if (!context.Users.Any(u => u.Email == "boris_ruskov@gmail.com"))
            {
                ApplicationUser seedUser2 = new ApplicationUser
                {
                    Name = "Boris Ruskov",
                    Email = "boris_ruskov@gmail.com",
                    UserName = "boris_ruskov@gmail.com"
                };
                userManager.Create(seedUser2, "Password2");
            }

            if (!context.Users.Any(u => u.Email == "mariya.stancheva@abv.bg"))
            {
                ApplicationUser seedUser3 = new ApplicationUser
                {
                    Name = "Mariya Stancheva",
                    Email = "mariya.stancheva@abv.bg",
                    UserName = "mariya.stancheva@abv.bg"
                };
                userManager.Create(seedUser3, "Password3");
            }

            context.SaveChanges();

            // Categories

            context.Categories.AddOrUpdate(
                c => c.Name,
                new Category
                {
                    Name = "Food",
                    IsDebit = true,
                    ApplicationUsers = new List<ApplicationUser>
                    {
                        context.Users.First(u => u.Email == "aia131@aubg.edu")
                    }
                },
                new Category
                {
                    Name = "Personal Care",
                    IsDebit = true,
                    ApplicationUsers = new List<ApplicationUser>
                    {
                        context.Users.First(u => u.Email == "aia131@aubg.edu")
                    }
                },
                new Category
                {
                    Name = "Wage",
                    IsDebit = false,
                    ApplicationUsers = new List<ApplicationUser>
                    {
                        context.Users.First(u => u.Email == "aia131@aubg.edu")
                    }
                });

            context.SaveChanges();

            // Transactions

            context.Transactions.AddOrUpdate(
                t => t.TransactionID,
                new Transaction
                {
                    Date = DateTime.Now,
                    Amount = 4.0,
                    Person = context.Users.First(u => u.Email == "aia131@aubg.edu"),
                    Category = context.Categories.First(c => c.Name == "Wage")
                },
                new Transaction
                {
                    Date = DateTime.Now.AddDays(-1),
                    Amount = 2.0,
                    Person = context.Users.First(u => u.Email == "aia131@aubg.edu"),
                    Category = context.Categories.First(c => c.Name == "Food")
                },
                new Transaction
                {
                    Date = DateTime.Now.AddDays(-2),
                    Amount = 1.0,
                    Note = "soap",
                    Person = context.Users.First(u => u.Email == "aia131@aubg.edu"),
                    Category = context.Categories.First(c => c.Name == "Personal Care")
                });

            context.SaveChanges();
        }
    }
}
