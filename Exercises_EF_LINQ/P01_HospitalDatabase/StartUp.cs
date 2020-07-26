using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data;

namespace P01_HospitalDatabase
{
    public class StartUp
    {
        static void Main()
        {
            var db = new HospitalContext();
            //db.Database.EnsureCreated();

            db.Database.Migrate();
        }
    }
}
