using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<CarImportDTO, Car>()
                .ForMember(x => x.PartCars, y => y.MapFrom(x => x.PartCars))
                .ReverseMap();

            this.CreateMap<CarExportWithAttributesDTO, Car>()
                .ReverseMap();

            this.CreateMap<Supplier, SupplierExportDTO>()
                .ForMember(x => x.PartsCount, y => y.MapFrom(x => x.Parts.Count))
                .ReverseMap();

            this.CreateMap<Part, PartForCarWithPartsExportDTO>()
                .ReverseMap();

        }
    }
}
