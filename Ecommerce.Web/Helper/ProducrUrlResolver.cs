using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Web.Dtos;

namespace Ecommerce.Web.Helper;

public class ProducrUrlResolver : IValueResolver<Product, ProductReturnDto, string>
{
    private readonly IConfiguration _config;

    public ProducrUrlResolver(IConfiguration config)
    {
        _config = config;
    }
    public string Resolve(Product source, ProductReturnDto destination, string destMember, ResolutionContext context)
    {
        if(!string.IsNullOrEmpty(source.PictureUrl))
        {
            return _config["ApiUrl"]+source.PictureUrl;
        }
        return null!;
    }
}
