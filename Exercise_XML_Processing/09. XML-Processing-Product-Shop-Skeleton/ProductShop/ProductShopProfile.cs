using AutoMapper;
using ProductShop.Dtos.Export;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<Product, ProductDTO>()
                .ForMember(x => x.Name, y => y.MapFrom(x => x.Name))
                .ForMember(x => x.Price, y => y.MapFrom(x => x.Price))
                .ForMember(x => x.BuyerFullName,
                            y => y.MapFrom(x => x.Buyer.LastName != null ? $"{x.Buyer.FirstName} {x.Buyer.LastName}" : null))
                .ReverseMap();
        }
    }
}
