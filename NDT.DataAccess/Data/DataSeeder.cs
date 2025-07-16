using Microsoft.AspNetCore.Identity;
using NDT.BusinessModels.Entities;

namespace NDT.DataAccess.Data
{
    public class DataSeeder
    {
        public static async Task SeedRolesAndUsersAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            string[] roleNames = { "Default", "Journalist", "Admin" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Default User
            var defaultUser = new AppUser
            {
                UserName = "defaultuser",
                Email = "default@example.com",
                FullName = "Default User",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(defaultUser.Email) == null)
            {
                var result = await userManager.CreateAsync(defaultUser, "Default123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(defaultUser, "Default");
                }
            }

            // Seed Journalist User
            var journalistUser = new AppUser
            {
                UserName = "journalist",
                Email = "journalist@example.com",
                FullName = "Journalist User",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(journalistUser.Email) == null)
            {
                var result = await userManager.CreateAsync(journalistUser, "Journalist123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(journalistUser, "Journalist");
                }
            }

            // Seed Admin User
            var adminUser = new AppUser
            {
                UserName = "admin",
                Email = "admin@example.com",
                FullName = "Admin User",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
} 