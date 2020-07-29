using PetStore.Models;
using PetStore.ServiceModels.Products;
using System.Collections.Generic;

namespace PetStore.Services.Contracts
{
    interface IProductService
    {
        void Add(Product product);

        Product CreateInputModel(string name, string productType, decimal price);

        IEnumerable<ProductOutputModel> ListSpecificType(string productType);

        IEnumerable<Product> ListAllByName();

        IEnumerable<Product> ListAllByType();

        IEnumerable<Product> ListAllByPrice();

        IEnumerable<Product> ListAllByOfficialId();

        int RemoveByName(string name);

        int RemoveByOficialId(int officialId);

        IEnumerable<ProductOutputModel> SearchByPartOfName(string partOfName);

        ProductOutputModel SearchByName(string name);

        ProductOutputModel SearchByOfficialId(int officialId);

        int EditProductDataByOfficialId(int officialId, string name, string productType, decimal price);
    }
}
