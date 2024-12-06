using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces;
public interface IProductRepository
{
    Task<Product> GetProductAsync(int id);
    Task<IReadOnlyList<Product>> GetProductsAsync(); 
    Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
    Task<IReadOnlyList<ProductType>> GetTypesAsync();
    Task<IReadOnlyList<ProductSubType>> GetSubTypesAsync();

}
