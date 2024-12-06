namespace Ecommerce.Core.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string PictureUrl { get; set; } = string.Empty;


    public int ProductBrandId { get; set; }
    public ProductBrand ProductBrand { get; set; }

    public int ProductSubTypeId { get; set; }
    public ProductSubType ProductSubType { get; set; }
}
