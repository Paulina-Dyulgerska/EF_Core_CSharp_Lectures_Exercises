using AutoMapper;
using ProductShop.DTOs.Category;
using ProductShop.DTOs.Product;
using ProductShop.DTOs.User;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<Product, ListProductsInRangeDTO>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name))
                .ForMember(x => x.Price, y => y.MapFrom(s => s.Price))
                .ForMember(x => x.Seller, y => y.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"))
                .ReverseMap();

            this.CreateMap<Product, UserSoldProductDTO>()
                .ForMember(x => x.BuyerFirstName, y => y.MapFrom(p => p.Buyer.FirstName))
                .ForMember(x => x.BuyerLastName, y => y.MapFrom(p => p.Buyer.LastName))
                .ReverseMap();

            this.CreateMap<User, UserWithSoldProductsDTO>()
                .ForMember(x => x.FirstName, y => y.MapFrom(u => u.FirstName))
                .ForMember(x => x.LastName, y => y.MapFrom(u => u.LastName))
                .ForMember(x => x.SoldProducts, y => y.MapFrom(u => u.ProductsSold.Where(x => x.BuyerId != null)))
                .ReverseMap();

            this.CreateMap<Category, CategoryWithProductsDTO>()
                .ForMember(x => x.CategoryName, y => y.MapFrom(x => x.Name))
                .ForMember(x => x.ProductsCount, y => y.MapFrom(x => x.CategoryProducts.Count))
                .ForMember(x => x.AveragePrice,
                    y => y.MapFrom(x => (x.CategoryProducts.Select(p => p.Product.Price).Sum() / x.CategoryProducts.Count).ToString("f2")))
                .ForMember(x => x.TotalRevenue, y => y.MapFrom(x => x.CategoryProducts.Select(p => p.Product.Price).Sum().ToString("f2")));
        }
    }
}
