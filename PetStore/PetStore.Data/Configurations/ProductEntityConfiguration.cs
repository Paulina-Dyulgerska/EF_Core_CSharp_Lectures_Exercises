using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetStore.Models;

namespace PetStore.Data.Configurations
{
   public class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //builder.HasAlternateKey(x=>x.Name); //taka si pravq name propertyto na Producta da e UNIQUE!!!!
            //tupo e da pravq taka, zashtoto si imam OfficialId, koeto shte polzwa za celite na Remove methodite!
        }
    }
}
