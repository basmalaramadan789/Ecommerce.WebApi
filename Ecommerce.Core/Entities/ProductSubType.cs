using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class ProductSubType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
    }
}
