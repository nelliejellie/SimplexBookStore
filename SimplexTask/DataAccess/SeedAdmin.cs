using Microsoft.AspNetCore.Identity;
using SimplexTask.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexTask.DataAccess
{
    public static class SeedAdmin
    {
        public async static Task Seed(
            AppDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManger
        )
        {
            dbContext.Database.EnsureCreated();

            if (!roleManager.Roles.Any())
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            var user = await userManger.FindByEmailAsync("emekaewelike@gmail.com");
            if(user is null)
            {
                var superUser = new AppUser
                {
                    Email = "emekaewelike@gmail.com",
                    UserName = "emekaewelike@gmail.com",
                };

                await userManger.CreateAsync(superUser, "Qwerty1234!");
                await userManger.AddToRoleAsync(superUser, "Admin");
            }
        }
    }
}
