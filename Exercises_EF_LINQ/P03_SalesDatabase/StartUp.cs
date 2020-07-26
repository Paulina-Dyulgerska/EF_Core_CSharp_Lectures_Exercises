using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data;

namespace P03_SalesDatabase
{
    class StartUp
    {
        static void Main()
        {
            var db = new SalesContext();
            db.Database.Migrate();
            //db.Database.EnsureCreated();
        }
    }
}
