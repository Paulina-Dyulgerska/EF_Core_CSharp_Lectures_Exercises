using RealEstates.Data;
using System;

namespace RealEstates.ConsoleApplication
{
    class Program
    {
        static void Main( )
        {
            var dbContext = new RealEstateContext();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }
}
