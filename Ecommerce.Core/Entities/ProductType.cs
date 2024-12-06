using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities;
public class ProductType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<ProductSubType> ProductSubTypes { get; set; } = new List<ProductSubType>();
}
