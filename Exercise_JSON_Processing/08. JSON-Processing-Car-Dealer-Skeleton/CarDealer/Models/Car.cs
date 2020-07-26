using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

        public virtual ICollection<PartCar> PartCars { get; set; } = new List<PartCar>();
    }
}