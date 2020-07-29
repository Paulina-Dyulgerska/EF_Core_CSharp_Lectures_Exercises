using AutoMapper;
using AutoMapper.QueryableExtensions;
using PetStore.Data;
using PetStore.Models;
using PetStore.ServiceModels.Products;
using PetStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Services
{
    public class ProductService : IProductService
    {
        private readonly PetStoreContext db;
        private readonly IMapper mapper;
        private int officialId;

        public ProductService(PetStoreContext dbContext, IMapper mapper)
        {
            this.db = dbContext;
            this.mapper = mapper;
        }

        public Product CreateInputModel(string name, string productType, decimal price)
        {
            var formattedName = this.FormatInputString(name);
            var formattedProductType = this.FormatInputString(productType);

            //Price
            if (price < 0)
            {
                throw new ArgumentException(nameof(price));
            }

            //ProductType
            var productTypeEntity = this.db.ProductTypes.FirstOrDefault(x => x.Type == formattedProductType);

            if (productTypeEntity == null)
            {
                productTypeEntity = new ProductType { Type = formattedProductType };
                this.db.ProductTypes.Add(productTypeEntity);
            }

            this.db.SaveChanges(); //to give an Id to the productTypeEntity!

            //Product
            var productEntity = this.db.Products.FirstOrDefault(x => x.Name == formattedName);

            if (productEntity != null)
            {
                throw new ArgumentException(nameof(name));
            }

            var product = new Product
            {
                OfficialId = GiveOfficialId(),
                Name = formattedName,
                ProductType = productTypeEntity,
                Price = price,
            };

            return product;
        }

        private string FormatInputString(string stringToFormat)
        {
            return $"{stringToFormat.ToUpper()[0]}{stringToFormat.Substring(1).ToLower()}";
        }

        public void Add(Product product)
        {
            this.db.Products.Add(product);

            this.db.SaveChanges();
        }

        public int RemoveByName(string name)
        {
            var productEntity = this.db.Products.FirstOrDefault(x => x.Name.ToLower() == name.Trim().ToLower());

            if (productEntity == null)
            {
                throw new ArgumentException(nameof(name));
            }

            this.db.Products.Remove(productEntity);

            return this.db.SaveChanges();
        }

        public int RemoveByOficialId(int offcialId)
        {
            var productEntity = this.db.Products.FirstOrDefault(x => x.OfficialId == offcialId);

            if (productEntity == null)
            {
                throw new ArgumentException(nameof(offcialId));
            }

            this.db.Products.Remove(productEntity);

            return this.db.SaveChanges();
        }

        private int GiveOfficialId()
        {
            this.officialId++;

            return this.officialId;
        }

        public IEnumerable<ProductOutputModel> ListSpecificType(string productType)
        {
            var productTypeEntity = this.db.ProductTypes.FirstOrDefault(x => x.Type.ToLower() == productType.Trim().ToLower());

            if (productTypeEntity == null)
            {
                throw new ArgumentException(nameof(productType));
            }

            var products = this.db.Products.Where(x => x.ProductType.Type.ToLower() == productType.Trim().ToLower());

            return products
                .ProjectTo<ProductOutputModel>(this.mapper.ConfigurationProvider)
                .OrderBy(x => x.Name)
                .ThenBy(x => x.OfficialId)
                .ToList();
        }

        public IEnumerable<Product> ListAllByName()
        {
            return this.db.Products
                .OrderBy(x => x.Name)
                .ThenBy(x => x.OfficialId)
                .ToList();
        }

        public IEnumerable<Product> ListAllByType()
        {
            return this.db.Products
                .OrderBy(x => x.ProductType.Type)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.OfficialId)
                .ToList();
        }

        public IEnumerable<Product> ListAllByPrice()
        {
            return this.db.Products
                .OrderBy(x => x.Price)
                .ThenBy(x => x.Name)
                .ThenBy(x => x.OfficialId)
                .ToList();
        }

        public IEnumerable<Product> ListAllByOfficialId()
        {
            return this.db.Products
                .OrderBy(x => x.OfficialId)
                .ToList();
        }

        public IEnumerable<ProductOutputModel> SearchByPartOfName(string partOfName)
        {
            var productEntities = this.db.Products
                .Where(x => x.Name.ToLower().Contains(partOfName.Trim().ToLower()));

            return productEntities.ProjectTo<ProductOutputModel>(this.mapper.ConfigurationProvider).ToList();
        }

        public ProductOutputModel SearchByName(string name)
        {
            var productEntity = this.db.Products
                .FirstOrDefault(x => x.Name.ToLower() == name.Trim().ToLower());

            if (productEntity == null)
            {
                throw new ArgumentException(nameof(name));
            }

            return this.mapper.Map<ProductOutputModel>(productEntity);
        }

        public ProductOutputModel SearchByOfficialId(int officialId)
        {
            var productEntity = this.db.Products
                .FirstOrDefault(x => x.OfficialId == officialId);

            if (productEntity == null)
            {
                throw new ArgumentException(nameof(officialId));
            }

            return this.mapper.Map<ProductOutputModel>(productEntity);
        }

        public int EditProductDataByOfficialId(int officialId, string name, string productType, decimal price)
        {
            var result = 0;

            try
            {
                var formattedName = this.FormatInputString(name);
                var formattedProductType = this.FormatInputString(productType);

                var productEntity = this.db.Products.FirstOrDefault(x => x.OfficialId == officialId);

                if (productEntity == null)
                {
                    throw new AggregateException(nameof(officialId));
                }

                if (productEntity.Name != formattedName)
                {
                    productEntity.Name = formattedName;
                }

                if (productEntity.ProductType.Type != formattedProductType)
                {
                    var productTypeEntity = this.db.ProductTypes.FirstOrDefault(x => x.Type == formattedProductType);

                    if (productTypeEntity == null)
                    {
                        productTypeEntity = new ProductType { Type = formattedProductType };
                        this.db.ProductTypes.Add(productTypeEntity);
                    }

                    this.db.SaveChanges(); //to give an Id to the productTypeEntity!

                    productEntity.ProductType = productTypeEntity;
                }

                if (productEntity.Price != price)
                {
                    productEntity.Price = price;
                }

                result = this.db.SaveChanges();
            }
            catch (ArgumentException ae) //ako polucha argument exception, da q predam natatyk, da ne q potusha tuk!
            {
                throw ae;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }
    }
}
