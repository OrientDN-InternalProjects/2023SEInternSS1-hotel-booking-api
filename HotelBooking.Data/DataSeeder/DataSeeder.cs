using HotelBooking.Common.Constants;
using HotelBooking.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Data.DataSeeder
{
    public class DataSeeder : IDataSeeder
    {
        private readonly ILogger<DataSeeder> logger;
        private readonly BookingDbContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public DataSeeder(ILogger<DataSeeder> logger, BookingDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.logger = logger;
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void Initialize()
        {
            AddAdministrator();
            context.SaveChanges();
        }
        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                var adminRole = new Role()
                {
                    Name = RoleConstant.AdminRole,
                    Description = "Administrator role with super-full permission"
                };

                var adminRoleInDb = await roleManager.FindByNameAsync(RoleConstant.AdminRole);
                if (adminRoleInDb == null)
                {
                    await roleManager.CreateAsync(adminRole);
                    logger.LogInformation("Seeded Administrator Role.");
                }

                var customerRole = new Role()
                {
                    Name = RoleConstant.CustomerRole,
                    Description = "Customer role with custom permission"
                };

                var employeeRoleInDb = await roleManager.FindByNameAsync(RoleConstant.CustomerRole);
                if (employeeRoleInDb == null)
                {
                    await roleManager.CreateAsync(customerRole);
                    logger.LogInformation("Seeded Employee Role.");
                }

                var superUser = new User()
                {
                    FullName = "Hana Ma Canada",
                    Email = "girlmilk123@gmail.com",
                    UserName = "superadmin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true,
                    PhoneNumber = "0971065997"
                };

                var superUserInDb = await userManager.FindByNameAsync(superUser.UserName);
                if (superUserInDb == null)
                {
                    await userManager.CreateAsync(superUser, UserConstant.DefaultPassword);
                    var result = await userManager.AddToRoleAsync(superUser, RoleConstant.AdminRole);
                    if (result.Succeeded)
                    {
                        logger.LogInformation("Seeded Default SuperAdmin User.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            logger.LogError(error.Description);
                        }
                    }
                }
            }).GetAwaiter().GetResult();
        }
    }
}
