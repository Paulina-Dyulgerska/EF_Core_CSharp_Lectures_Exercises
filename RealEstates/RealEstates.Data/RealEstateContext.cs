using Microsoft.EntityFrameworkCore;
using RealEstates.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstates.Data
{
    public class RealEstateContext : DbContext
    {
        public RealEstateContext()
        {
        }

        public RealEstateContext(DbContextOptions options)
            : base(options)
        {
        }

        DbSet<RealEstateProperty> RealEstateProperties { get; set; }

        public int MyProperty { get; set; }
    }
}
