using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Data;
using PetStore.Mapping;
using PetStore.Models;
using PetStore.Services;

namespace PetStore.ConsoleApplication
{
    class Program
    {
        static void Main()
        {
            var dbContext = new PetStoreContext();
            dbContext.Database.Migrate();

            var config = new MapperConfiguration(cnf =>
            {
                cnf.AddProfile(new PetStoreProfile());
            });
            IMapper mapper = config.CreateMapper();

            var service = new ProductService(dbContext, mapper);

            var product = service.CreateInputModel("Bag For Dogs", "Bags", 23.00m);
            service.Add(product);
        }
    }
}
