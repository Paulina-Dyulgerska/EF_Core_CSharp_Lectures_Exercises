using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Models;
using RealEstates.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace RealEstates.Services
{
    public class RealEstatePropertiesService : IRealEstatePropertiesService
    {
        private RealEstateContext db;

        public RealEstatePropertiesService(RealEstateContext db)
        //za da imam dependency invertion, podavam vsichko, koeto
        //mu trqbwa na tozi class, da idva otvyn, prez constructora!!!
        //az mojeh tuk da si napravq this.db = new RealEstateContext(), no towa
        //shteshe da mi zakove da rabotq samo s tozi konkreten DbContext i ako iskam da go smenq utre, trqbwa
        //da vlizam i da promenqm i tuk i navsqkyde. Mnogo po-chisto e da mi dojde otvyn, ot tozi kojto iska
        //da raboti s tozi class, da dojde infoto za DbCOntexta!!!
        {
            this.db = db;
        }

        public void Create(string district, int size, int? year, int price, string propertyType,
            string buildingType, int? floor, int? maxFloor)
        {
            //Hubavo e Service - te da dobavqt vsichko nakup pri nachalnoto Seed - vane na danni v DB-a, a ne da dobavqt edin sled drug 23000 zapisa, kakto
            //az napravih.Trqbwashe da imam method InitialSeed i da se kachi vsichko v dbContext-a, a edva togawa da dam SaveChanges(). A az ostavih
            //sled methoda Create() da se pravi SaveChanges() pri vseki nov dobaven zapis, koeto e OK za normalnata rabota na prilojenieto mi, no ne e
            //ako za pyrvonachalnoto mi Seed-vane na danni!!!!!Da go mislq drug pyt.

            if (district == null)
            {
                throw new ArgumentNullException(nameof(district));
            }

            var realEstateProperty = new RealEstateProperty
            {
                Size = size,
                Price = price,
                //Year = year < 1800 ? null : year, //godinata e nevalidna i slagam null!
                //dolu shte proverqwam, a ne tuk, za da pokaja i drugiq nachin towa da stane.
                Year = year,
                Floor = floor,
                TotalFloors = maxFloor,
            };

            //Year
            if (year < 1800)
            {
                realEstateProperty.Year = null;
            }

            //Floor
            if (floor < 0)
            {
                realEstateProperty.Floor = null;
            }

            //TotalNumberOfFloors
            if (maxFloor < 0)
            {
                realEstateProperty.TotalFloors = null;
            }

            //District
            District districtEntity = this.db.Districts.FirstOrDefault(x => x.Name.Trim() == district.Trim());
            if (districtEntity == null)
            {
                districtEntity = new District { Name = district.Trim() };
            }

            realEstateProperty.District = districtEntity;
            //kakvo pravi EF s gorniq kod: ako ima takyv district, vzima mu id-to i go slaga na moeto novo
            //realEstateProperty.District kato DistrictId, a ako nqma takyv district, EF shte vidi, 
            //che go sydawam v if-a i shte go sloji v
            //DB-a (pyrvo v tracker-a si shte go sloji i pri SaveChanges() shte go sloji v DB-a), kato EF shte mu
            //dade id (sledwashtoto po red v tablica Districts), shte zapomni towa id i tochno 
            //tova id posle shte zapishe v 
            //realEstateProperty.District kato DistrictId!!!!

            //PropertyType
            PropertyType propertyTypeEntity = this.db.PropertyTypes
                .FirstOrDefault(x => x.Name.Trim() == propertyType.Trim());
            if (propertyTypeEntity == null)
            {
                propertyTypeEntity = new PropertyType { Name = propertyType.Trim() };
            }

            realEstateProperty.PropertyType = propertyTypeEntity;

            //BuildingType
            BuildingType buildingTypeEntity = this.db.BuildingTypes
                .FirstOrDefault(x => x.Name.Trim() == buildingType.Trim());
            if (buildingTypeEntity == null)
            {
                buildingTypeEntity = new BuildingType { Name = buildingType.Trim() };
            }

            realEstateProperty.BuildingType = buildingTypeEntity;

            this.db.RealEstateProperties.Add(realEstateProperty);
            this.db.SaveChanges();

            //Tags tuk pravq, zashtoto EF sled SaveChages() veche ima id-to na syzdawanoto tuk realEstateProperty!
            //predi towa go nqma towa id i az ne moga da slagam tags s moq method UpdateTags(int realEstatePropertyId)
            this.UpdateTags(realEstateProperty.Id);
        }

        public IEnumerable<PropertyViewModel> Search(int minYear, int maxYear, int minSize, int maxSize)
        {
            return db.RealEstateProperties.Where(x => x.Year >= minYear && x.Year <= maxYear
                                                        && x.Size >= minSize && x.Size <= maxSize)
                .Select(MapToRealEstatePropertyViewModel())
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Size)
                .ToList();
        }

        public IEnumerable<PropertyViewModel> SearchByPrice(int minPrice, int maxPrice)
        {
            return this.db.RealEstateProperties.Where(x => x.Price >= minPrice && x.Price <= maxPrice)
                .Select(MapToRealEstatePropertyViewModel())
                .OrderBy(x => x.Price)
                .ToList();
        }

        public void UpdateTags(int realEstatePropertyId)
        {
            var realEstateProperty = this.db.RealEstateProperties.FirstOrDefault(x => x.Id == realEstatePropertyId);

            realEstateProperty.Tags.Clear();
            //izchistvam vsichki nalichni tagove, za da ne mi se dublirat s novite, koito az shte syzdam!

            if (realEstateProperty.Year.HasValue && realEstateProperty.Year < 1900)
            {
                realEstateProperty.Tags.Add(new RealEstatePropertyTag
                {
                    Tag = GetOrCreateTag("OldBuilding"),
                });
            }

            if (realEstateProperty.Size > 120)
            {
                realEstateProperty.Tags.Add(new RealEstatePropertyTag
                {
                    Tag = GetOrCreateTag("HugeApartment"),
                });
            }

            if (realEstateProperty.Floor.HasValue && realEstateProperty.Floor > 5)
            {
                realEstateProperty.Tags.Add(new RealEstatePropertyTag
                {
                    Tag = GetOrCreateTag("HighFloor"),
                });
            }

            if (realEstateProperty.District.Name == "Стрелбищe")
            {
                realEstateProperty.Tags.Add(new RealEstatePropertyTag
                {
                    Tag = GetOrCreateTag("Central"),
                });
            }

            if (realEstateProperty.Floor.HasValue && realEstateProperty.Floor == 0)
            {
                realEstateProperty.Tags.Add(new RealEstatePropertyTag
                {
                    Tag = GetOrCreateTag("GroundFloor"),
                });
            }

            if (realEstateProperty.Floor.HasValue && realEstateProperty.Floor == realEstateProperty.TotalFloors)
            {
                realEstateProperty.Tags.Add(new RealEstatePropertyTag
                {
                    Tag = GetOrCreateTag("LastFloor"),
                });
            }

            if (realEstateProperty.TotalFloors > 5
                && realEstateProperty.Year.HasValue
                && realEstateProperty.Year > 2018)
            {
                realEstateProperty.Tags.Add(new RealEstatePropertyTag
                {
                    Tag = GetOrCreateTag("HasParking"),
                });
            }

            if (realEstateProperty.Price / realEstateProperty.Size < 800)
            {
                realEstateProperty.Tags.Add(new RealEstatePropertyTag
                {
                    Tag = GetOrCreateTag("CheapProperty"),
                });
            }

            if (realEstateProperty.Price / realEstateProperty.Size > 2000)
            {
                realEstateProperty.Tags.Add(new RealEstatePropertyTag
                {
                    Tag = GetOrCreateTag("ExpensiveProperty"),
                });
            }

            this.db.SaveChanges();
        }

        private Tag GetOrCreateTag(string tag)
        {
            var tagEntity = this.db.Tags.FirstOrDefault(x => x.Name.Trim() == tag.Trim());

            if (tagEntity == null)
            {
                tagEntity = new Tag { Name = tag.Trim() };
                this.db.Tags.Add(tagEntity);
            }

            return tagEntity;
        }

        private static Expression<Func<RealEstateProperty, PropertyViewModel>> MapToRealEstatePropertyViewModel()
        {
            return x => new PropertyViewModel
            {
                District = x.District.Name,
                Size = x.Size,
                Year = x.Year,
                Price = x.Price,
                PropertyType = x.PropertyType.Name,
                BuildingType = x.BuildingType.Name,
                Floor = ((x.Floor ?? 0) + '/' + (x.TotalFloors ?? 0)).ToString(),
            };
        }

    }
}
