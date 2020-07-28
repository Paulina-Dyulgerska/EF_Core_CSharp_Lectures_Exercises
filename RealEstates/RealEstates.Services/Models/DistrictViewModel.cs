namespace RealEstates.Services.Models
{
    public class DistrictViewModel
    {
        public string Name { get; set; }

        public int minPrice { get; set; }

        public int maxPrice { get; set; }

        public double AveragePrice { get; set; }

        public double AveragePricePerSquareMeter { get; set; }

        public int RealEstatePropertiesCount { get; set; }
    }
}