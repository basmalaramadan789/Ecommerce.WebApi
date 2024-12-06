using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications;
public class ProductWithFilterForCount : BaseSpecification<Product>
{
    public ProductWithFilterForCount(ProductSpecParams productParams)
        : base(x =>
         (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
         (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
         (!productParams.TypeId.HasValue || x.ProductSubTypeId == productParams.TypeId))

    {

    }
}
