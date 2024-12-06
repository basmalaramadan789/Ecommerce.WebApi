using Ecommerce.Core.Entities;

namespace Ecommerce.Web.Dtos;

public class ProductReturnDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;

    public string ProductType { get; set; }
    public string ProductBrand { get; set; }
    public string ProductSubType { get; set; }
}
