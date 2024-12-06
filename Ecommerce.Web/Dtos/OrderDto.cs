namespace Ecommerce.Web.Dtos;

public class OrderDto
{
    public string BasketId {  get; set; }
    public int DeleveryMethodId {  get; set; }
    public AddressDto ShipToAddress {  get; set; }
}
