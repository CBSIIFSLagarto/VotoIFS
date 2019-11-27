using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Core_RBS.Models;
using Microsoft.AspNetCore.Identity;

namespace Core_RBS.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.Migrate();
            SeedData(userManager, roleManager);
        }

        public static void SeedData (UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers (UserManager<Usuario> userManager)
        {
            foreach (var r in regras)
            {
                if (userManager.FindByNameAsync(r).Result == null)
                {
                    var user = new Usuario();
                    user.UserName = user.Email = $"{r}@localhost.com".ToLower();
                    user.IsAdmin = true;
                    user.Nome = r;

                    IdentityResult result = userManager.CreateAsync(user, "123qwe").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, r).Wait();
                    }
                }
            }
        }
        private static string[] regras = new String[] { "Administrador", "Professor" };

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var r in regras) {
                if (!roleManager.RoleExistsAsync(r).Result)
                {
                    IdentityRole role = new IdentityRole();
                    role.Name = r;
                    role.NormalizedName = r;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }
    }
}
