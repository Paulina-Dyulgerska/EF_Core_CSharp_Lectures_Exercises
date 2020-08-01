namespace RealEstates.Services.Models
{
    public class DistrictViewModel
    {
        public string Name { get; set; }

        public int MinPrice { get; set; }

        public int MaxPrice { get; set; }

        public double AveragePrice { get; set; }

        public double AveragePricePerSquareMeter { get; set; }

        public int RealEstatePropertiesCount { get; set; }
    }
}