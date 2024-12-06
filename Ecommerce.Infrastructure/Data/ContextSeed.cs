using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Ecommerce.Infrastructure.Data;

public class ContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Ensure the database is created
        context.Database.EnsureCreated();

        // Seed ProductBrands first
        if (!context.productBrands.Any())
        {
            var brandsData = File.ReadAllText("../Ecommerce.Infrastructure/Data/SeedData/brands.json");
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            context.productBrands.AddRange(brands);
            await context.SaveChangesAsync(); // Save changes to ensure brands are inserted
        }

        // Seed ProductTypes next
        if (!context.ProductTypes.Any())
        {
            var typsData = File.ReadAllText("../Ecommerce.Infrastructure/Data/SeedData/types.json");
            var typs = JsonSerializer.Deserialize<List<ProductType>>(typsData);
            context.ProductTypes.AddRange(typs);
            await context.SaveChangesAsync(); // Save changes to ensure types are inserted
        }
        if (!context.ProductSubTypes.Any())
        {
            var subtypesData = File.ReadAllText("../Ecommerce.Infrastructure/Data/SeedData/subtypes.json");
            var subtypes = JsonSerializer.Deserialize<List<ProductSubType>>(subtypesData);
            context.ProductSubTypes.AddRange(subtypes);
            await context.SaveChangesAsync(); // Save changes to ensure subtypes are inserted
        }

        // Seed Products last
        if (!context.Products.Any())
        {
            var productsData = File.ReadAllText("../Ecommerce.Infrastructure/Data/SeedData/products.json");
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            context.Products.AddRange(products);
            await context.SaveChangesAsync(); // Save changes to ensure products are inserted
        }
        if (!context.DeliveryMethods.Any())
        {
            var deleveryData = File.ReadAllText("../Ecommerce.Infrastructure/Data/SeedData/delivery.json");
            var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deleveryData);
            context.DeliveryMethods.AddRange(methods);
            await context.SaveChangesAsync(); // Save changes to ensure products are inserted
        }
        // Check if there are any changes to save
        if (context.ChangeTracker.HasChanges())
        {
            await context.SaveChangesAsync();
        }
    }
}

