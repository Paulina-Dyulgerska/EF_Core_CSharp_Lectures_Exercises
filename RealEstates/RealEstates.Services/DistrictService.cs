using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RealEstates.Services
{
    public class DistrictService : IDistrictService
    {
        private readonly RealEstateContext db;
  
        public DistrictService(RealEstateContext dbContext)
        {
            this.db = dbContext;
        }

        public IEnumerable<DistrictViewModel> GetTopDistrictsByAveragePrice(int count = 10)
        {
            return this.db.Districts
                .Select(MapToDistrictViewModel())
                .OrderByDescending(d => d.AveragePricePerSquareMeter)
                .ThenBy(d => d.Name)
                .Take(count)
                .ToList();
        }

        public IEnumerable<DistrictViewModel> GetTopDistrictsByNumberOfProperties(int count = 10)
        {
            return db.Districts
                .Select(MapToDistrictViewModel())
                .OrderByDescending(d => d.RealEstatePropertiesCount)
                .ThenByDescending(d=>d.AveragePricePerSquareMeter)
                .ThenBy(d=>d.Name)
                .Take(count)
                .ToList();
        }

        private static Expression<Func<District, DistrictViewModel>> MapToDistrictViewModel()
        {
            return d => new DistrictViewModel
            {
                Name = d.Name,
                AveragePrice = d.RealEstateProperties.Average(p => p.Price), 
                AveragePricePerSquareMeter = d.RealEstateProperties.Average(p => (double)p.Price / p.Size),
                maxPrice = d.RealEstateProperties.Max(p => p.Price),
                minPrice = d.RealEstateProperties.Min(p => p.Price),
                RealEstatePropertiesCount = d.RealEstateProperties.Count,
            };
        }
    }
}
