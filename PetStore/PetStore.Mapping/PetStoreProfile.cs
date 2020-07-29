using AutoMapper;
using PetStore.Models;
using PetStore.ServiceModels.Products;

namespace PetStore.Mapping
{
    public class PetStoreProfile : Profile
    {
        public PetStoreProfile()
        {
            this.CreateMap<Product, ProductOutputModel>()
                .ForMember(x => x.ProductType, y => y.MapFrom(x => x.ProductType.Type));


        }
    }
}
