using Lecture_EF_EntityRelations.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Lecture_EF_EntityRelations.EntityConfigurations
{
    public class RecipeConfigurations : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            //builder.Property(x => x.Name).HasColumnName("Title");  //[Column("Title")] 

            builder.Property("Name")
            //.HasMaxLength(50) // [MaxLength(50)]
            .IsUnicode(true) //po default e Unicode, zatowa tozi red e izlishen
            .IsRequired(true); //s attribut [Requered] tozi red e izlishen

            //builder.HasKey("Name"); //[Key]

            builder.ToTable("MyRecipes", "system"); //[Table("MyRecipes", Schema="system")]

            builder.Ignore("Test"); //[NotMapped]

            //towa sa Trite zadyljitelni neshta, koito da prawq, za da sa mi pravilni vryzkite one-to-many:
            builder
                .HasMany(x => x.Ingredients) //tova e vajno za EF Core
                .WithOne(x => x.Recipe) //tova e vajno za EF Core
                .HasForeignKey(x=>x.RecipeId) //tova e vajno za DB i e fizicheskata vryzka v DB-a!!!!
                .OnDelete(DeleteBehavior.Restrict);

            ////builder.HasKey(new[] { "Id", "Name" }); //taka se pravi Composite Key
            ////builder.HasKey("Id", "Name"); //taka se pravi Composite Key
            //builder.HasKey(x=> new { x.Id, x.Name }); //taka se pravi Composite Key

            builder.HasIndex(x => x.Name).IsUnique(); //shte se napravi index za tyrsene po tazi kolona, koeto 
            //index shte e unikalen!!!

            //shawol property:
            builder.Property<DateTime>("LastUpdated");

            builder.Property<string>("EGN")
                .HasColumnType("nvarchar(10)");

            //izneseni sa nastrojkite za tablica Recipes v RecipeConfigurations.cs file!!!!
            //ako ne bqha izneseni, towa shteshe da e nalichno v modelBuilder-a v RecipesDbContext.cs file!!!!
            //modelBuilder.Entity<Recipe>(entity => //kazwam za koq tablica se otnasqt promenite, shte e table Recipes zasegnata!!
            //{
            //    entity.Property(x => x.Name).HasColumnName("Title");
            //    //sega vmesto Name, kolonata zad property Name shte se kazwa Title v DB-a mi!!!

            //    //.HasColumnType("char(30)") //moga da smenq defaultniq type na colonata Name ot tuk.
            //    //no obiknoveno e typo taka da se vyrja s konkreten type na konkreten type DB, zashtoto ne sym 
            //    //flexy ako smenq bazata utre. zatowa pisha taka:
            //    //.HasColumnType("varchar") //stremq se kym abstraction i v izbora na type - sega iskam NVARCHAR(100).
            //    //no mi hvyrlq greshka i ne dawa da se pravi sega promqna v type na Title colonata. Da vnimawam kak go
            //    //omotwam v tezi types, che sega mi iska da naprawi varchar(1) samo da e colonata.

            //    entity.Property("Name") //kazwam za koe property prawq nadolu promenite!!!!
            //    .HasMaxLength(100)
            //    .IsUnicode(true)
            //    .IsRequired(true); //ne moje da e NULL vypreki, che type go pozwolqwa, zashtoto e nullable stringa.

            //    entity.HasKey("Name");
            //    //moga da si pisha koqto si iskam kolona da e PK!!!

            //    entity.ToTable("MyRecipes", "system");
            //    //shte smenq imeto na tablicata ot Recipes na MyRecipes!!! Sega v DB imam:
            //    //[system].[MyRecipes] vmesto [dbo].Recipes!!!!
            //    //t.e. smebih i schemata, v koqto e tablicata.

            //    //entity.HasNoKey();  //ako iskam da nqma Primary Key edna table, az trqbwa izrishno da kaja towa
            //    //taka sega tablicata ostawa bez Primary Key, no ot tuk natatyk tablicata NE moje da se trackva
            //    //ot EF Core i zabrawqm za promeni v neq!!!! Moga samo da q cheta!!!
            //    //zatowa po princip ne iskam da prawq takiwa nestha, tochno zashtoto EF nqma kak da znae koe da trie i 
            //    //obrabotwa, ako nqma id ili index, po kojto da sledi koe zapische trqbva da se promeni.

            //    entity.Ignore("Test");
            //    //propertyto Test ne go iskam da e kolona v DB-a!!!! TO shte si ostane samo 
            //    //kato localno property i nqma da wliza w modela.

            //    //    //Disabling cascade delete:
            //    //    //If a FK property is non - nullable, cascade delete is on by default:
            //    //    //modelBuilder.Entity<Course>()
            //    //    //  .HasRequired(t => t.Department)
            //    //    //  .WithMany(t => t.Courses)
            //    //    //  .HasForeignKey(d => d.DepartmentID) //Throws exception on delete
            //    //    //  .OnDelete(DeleteBehavior.Restrict);
            //    //    entity.HasMany(x => x.Ingredients)
            //    //    .WithOne(x => x.Recipe)
            //    //    //.OnDelete(DeleteBehavior.Cascade) //razreshavam cascadno iztrivane, t.e. triq receptata i se triqt
            //    //    //vsichki vyrzani za neq ingredients!!! towa ne go iskam i e opasno - moga da si iztriq cqlata DB!!!
            //    //    .OnDelete(DeleteBehavior.Restrict); //zabranqwam iztriwane na recepta, dokato e
            //    //    //s vyrzani za neq Ingredients - trqbwa da sloja null v ingredientite i posle da triq recipe-to!!!
            //    //    //Throws exception on delete
            //    //     //.OnDelete(DeleteBehavior.SetNull); //ako e nullable colonata, koqto e FK, moga da razresha da i
            //    //    //se setne null, t.e. v property Recipe na Ingredientite koito imat tazi Recipe zakachena, shte se 
            //    //    //zapishe null pri delete-vaneto na tazi Recipe i nqma da mrynka DB-a za nishto.
            //});
        }
    }
}
