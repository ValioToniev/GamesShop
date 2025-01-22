using GamesShop.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesShop.Infrastructure.Data.Infrastructure
{
    public static class ApplicationBuilderExtension
    {
        public static async Task<IApplicationBuilder> PrepareDatabase(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            await RoleSeeder(services);
            await SeedAdministrator(services);

            var dataCategory = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedCategories(dataCategory);

            var dataGenre = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            SeedGenres(dataGenre);


            return app;
        }

        private static async Task RoleSeeder(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Administrator", "Client" };

            IdentityResult roleResult;

            foreach (var role in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        private static async Task SeedAdministrator(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (await userManager.FindByNameAsync("admin") == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    FirstName = "Valentin",
                    LastName = "Valentinov",
                    UserName = "Valentin777",
                    Email = "v.valentinov2020@pmg-pernik.com",
                    Address = "Pernik",
                    PhoneNumber = "0887337033"
                };

                var result = await userManager.CreateAsync(user, "Admin123456");

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }
        private static void SeedCategories(ApplicationDbContext dataCategory)
        {
            if (dataCategory.Categories.Any())
            {
                return;
            }

            dataCategory.Categories.AddRange(new[]
            {
        new Category { CategoryName = "Игра за PlayStation" },
        new Category { CategoryName = "Игра за Xbox" },
        new Category { CategoryName = "Игра за Nintendo Switch" },
        new Category { CategoryName = "Игра за PC" },
        
    });

            dataCategory.SaveChanges();
        }
        private static void SeedGenres(ApplicationDbContext dataGenre)
        {
            if (dataGenre.Genres.Any())
            {
                return;
            }

            dataGenre.Genres.AddRange(new[]
            {
        new Genre { GenreName = "Action" },
        new Genre { GenreName = "Adventure" },
        new Genre { GenreName = "Fantasy" },
        new Genre { GenreName = "Horror" },
        new Genre { GenreName = "Strategy" },
        new Genre { GenreName = "Sports" },
        new Genre { GenreName = "Battle Royale" },
        new Genre { GenreName = "Shooter" },
    });

            dataGenre.SaveChanges();
        }



    }

}
