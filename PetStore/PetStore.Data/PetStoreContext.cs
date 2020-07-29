using Microsoft.EntityFrameworkCore;
using PetStore.Common;
using PetStore.Models;

namespace PetStore.Data
{
    public class PetStoreContext : DbContext
    {
        public PetStoreContext()
        {
        }

        public PetStoreContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Pet> Pets { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Breed> Breeds { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(DbConfiguration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetStoreContext).Assembly);
            //taka mu kazwam da applyne vsichki nalichni configurations, koito gi imam v towa assembly, v
            //koeto mi se namira PetStoreContext-a!!!! Taka si spestqwam pisaneto pootdelno na
            //vsqko edin configuration failche, kakto go pravq, ako gi opisvam edno po edno taka:
            //modelBuilder.ApplyConfiguration(new ClientProductEntityConfiguration());
            //modelBuilder.ApplyConfiguration(new ClientEntityConfiguration());
        }
    }
}
