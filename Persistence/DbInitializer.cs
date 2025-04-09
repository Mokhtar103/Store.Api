using Domain.Contract;
using Domain.Entities;
using Persistence.Data;
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

        public DbInitializer(StoreDbContext context)
        {
            _context = context;
        }
        public async Task InitiliazeAsync()
        {
            try
            {
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
            }
            catch (Exception ex)
            {

            }
        }
    }
}
