namespace blog.Migrations
{
    using blog.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<blog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        private string RandomPhoneNumber()
        {
            var rand = new Random();
            return $"({rand.Next(100, 1000)})-{rand.Next(100, 1000)}-{rand.Next(1000, 10000)}";
        }
        protected override void Seed(blog.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if(!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }




            var userManager = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "AndrewRussell@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "AndrewRussell@coderfoundry.com",
                    UserName = "AndrewRussell@coderfoundry.com",
                    FirstName = "Andrew",
                    LastName = "Russell",
                    DisplayName = "The Duke",
                    AvatarPath = "/Avatars/DemoUserAvatars/Default_Avatar.png",
                    PhoneNumber = RandomPhoneNumber()

                }, 
                "HelloNurse!");


            }
            var userId = userManager.FindByEmail("AndrewRussell@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Moderator");


            if (!context.Users.Any(u => u.Email == "thomas.j.zanis@gmail.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "thomas.j.zanis@gmail.com",
                    UserName = "thomas.j.zanis@gmail.com",
                    FirstName = "Thomas",
                    LastName = "Zanis",
                    DisplayName = "The King",
                    AvatarPath = "/Avatars/DemoUserAvatars/2.jpg",
                    PhoneNumber = RandomPhoneNumber()

                },
                "Tobeornot123!");


            }
             userId = userManager.FindByEmail("thomas.j.zanis@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");

            if (!context.Users.Any(u => u.Email == "moderator@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "moderator@coderfoundry.com",
                    UserName = "moderator@coderfoundry.com",
                    FirstName = "moderator",
                    DisplayName = "The Lord",
                    AvatarPath = "/Avatars/DemoUserAvatars/Default_Avatar.png",
                    PhoneNumber = RandomPhoneNumber()

                },
                "123456Abc$");


            }
            userId = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Moderator");
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            #region Loading Categories
            context.Categories.AddOrUpdate(
                b => b.Name,
                new Category { Name = "C#.Net", Description = "Includes C# code" },
                new Category { Name = "JavaScrip", Description = "Includes CJavaScripcode" },
                new Category { Name = "CSS", Description = "Includes CSS code" },
                new Category { Name = "Html", Description = "Includes Html code" },
                new Category { Name = "CoderLife", Description = "Includes info about coding life" }
                );
            #endregion

        }
    }
}
