namespace Ecommerce.Web.Dtos;

public class WishListItemDto
{
    public int Id { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public decimal ProductPrice { get; set; }
}