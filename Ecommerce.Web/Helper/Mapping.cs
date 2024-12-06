using AutoMapper;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.OrderAggregate;
using Ecommerce.Core.Identity;
using Ecommerce.Web.Dtos;
using Address = Ecommerce.Core.Identity.Address;

namespace Ecommerce.Web.Helper;

public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<Product, ProductReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                //.ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProducrUrlResolver>());

        CreateMap<Address, AddressDto>().ReverseMap();

        CreateMap<AddressDto, Ecommerce.Core.Entities.OrderAggregate.Address>();

        CreateMap<Order, OrderToReturnDto>()
            .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
            .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());


        CreateMap<ProductCreateDto, Product>()
            .ForMember(d => d.ProductBrandId, o => o.MapFrom(s => s.ProductBrandId))
            //.ForMember(d => d.ProductTypeId, o => o.MapFrom(s => s.ProductTypeId))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.PictureUrl));

        CreateMap<ApplicationUser, UserDetailsDto>()
           .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
           .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.DisplayName))
           .ForMember(d => d.Address, o => o.MapFrom(s => s.Address));

    }
}
