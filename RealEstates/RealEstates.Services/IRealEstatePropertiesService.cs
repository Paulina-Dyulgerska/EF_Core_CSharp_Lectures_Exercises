using RealEstates.Services.Models;
using System.Collections;
using System.Collections.Generic;

namespace RealEstates.Services
{
    public interface IRealEstatePropertiesService
    {
        void Create(string district, int size, int? year, int price, string propertyType,
           string buildingType, int? floor, int? maxFloor);

        void UpdateTags(int realEstatePropertyId);

        IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int minSize, int maxSize);

        IEnumerable<PropertyViewModel> SearchByPrice(int minPrice, int maxPrice);
    }
}
