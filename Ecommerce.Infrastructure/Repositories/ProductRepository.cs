using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context) => _context = context;

    public async Task<Product> GetProductAsync(int id)
    {
        return await _context.Products
            //.Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductSubType)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync() // Matches interface
    {
        return await _context.Products
            //.Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            .Include(p => p.ProductSubType)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
    {
        return await _context.productBrands.ToListAsync();
    }

    public async Task<IReadOnlyList<ProductType>> GetTypesAsync()
    {
        return await _context.ProductTypes
            .Include(t => t.ProductSubTypes)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ProductSubType>> GetSubTypesAsync() // Matches interface
    {
        return await _context.ProductSubTypes.ToListAsync();
    }
    

}

