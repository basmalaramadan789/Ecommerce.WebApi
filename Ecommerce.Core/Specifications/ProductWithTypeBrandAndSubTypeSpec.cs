using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications;
public class ProductWithTypeBrandAndSubTypeSpec : BaseSpecification<Product>
{
    public ProductWithTypeBrandAndSubTypeSpec(ProductSpecParams productParams)
        : base(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
            (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
            (!productParams.TypeId.HasValue || x.ProductSubTypeId == productParams.TypeId) &&
            (!productParams.SubTypeId.HasValue || x.ProductSubTypeId == productParams.SubTypeId))
    {
        //AddIncludes(x => x.ProductType);
        AddIncludes(x => x.ProductBrand);
        AddIncludes(x => x.ProductSubType);

        ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

        if (!string.IsNullOrEmpty(productParams.Sort))
        {
            switch (productParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
    }

    public ProductWithTypeBrandAndSubTypeSpec(int id)
        : base(x => x.Id == id)
    {
        //AddIncludes(x => x.ProductSubType);
        AddIncludes(x => x.ProductBrand);
        AddIncludes(x => x.ProductSubType);
    }
}
