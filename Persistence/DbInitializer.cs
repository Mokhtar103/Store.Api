using Domain.Contract;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Entities.OrderEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;
        private readonly StoreIdentityDbContext _identityDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public DbInitializer(StoreDbContext context,
            StoreIdentityDbContext identityDbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _context = context;
           _identityDbContext = identityDbContext;
           _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task InitiliazeAsync()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                    await _context.Database.MigrateAsync();
               

                if (!_context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText(@"../Persistence/Data/Seeding/types.json");

                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    if (types is not null && types.Any())
                    {
                       await _context.ProductTypes.AddRangeAsync(types);

                        await _context.SaveChangesAsync();
                    }
                }

                if (!_context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText(@"../Persistence/Data/Seeding/brands.json");

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    if (brands is not null && brands.Any())
                    {
                        await _context.ProductBrands.AddRangeAsync(brands);

                       await _context.SaveChangesAsync();
                    }
                }

                if (!_context.Products.Any())
                {
                    var productsData = File.ReadAllText(@"../Persistence/Data/Seeding/products.json");

                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    if (products is not null && products.Any())
                    {
                       await _context.Products.AddRangeAsync(products);

                        await _context.SaveChangesAsync();
                    }
                }

                if (!_context.DeliveryMethods.Any())
                {
                    var deliverydata = File.ReadAllText(@"../Persistence/Data/Seeding/delivery.json");

                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliverydata);

                    if (deliveryMethods is not null && deliveryMethods.Any())
                    {
                        await _context.DeliveryMethods.AddRangeAsync(deliveryMethods);

                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task InitiliazeIdentityAsync()
        {
            if (_identityDbContext.Database.GetPendingMigrations().Any())
                await _identityDbContext.Database.MigrateAsync();

            if(!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            if (!_userManager.Users.Any())
            {
                var superAdminUser = new User
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "1234567890",
                };

                var adminUser = new User
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "987654321",
                };

                await _userManager.CreateAsync(superAdminUser, "Passw0rd");
                await _userManager.CreateAsync(adminUser, "Passw0rd");

                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }

        }
    }
}
