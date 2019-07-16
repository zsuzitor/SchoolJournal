using Microsoft.AspNetCore.Identity;
using SchoolJournal.Models;
using SchoolJournal.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.Data
{
    public static class ApplicationDbInitialize
    {
        public async static void Initialize(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            foreach (AppUserRole roleName in (AppUserRole[])Enum.GetValues(typeof(AppUserRole)))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName.ToString()));
            }
            var user = new ApplicationUser { UserName = "Admin@mail.ru", Email = "Admin@mail.ru" };
            var result = await userManager.CreateAsync(user, "Admin1!");
            await userManager.AddToRoleAsync(user,AppUserRole.Admin.ToString());

        }

    }
}
