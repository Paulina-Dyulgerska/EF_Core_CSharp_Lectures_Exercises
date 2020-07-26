using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lecture_EF_EntityRelations.Models
{
   public class Recipe
    {
        public Recipe()
        {
            this.Ingredients = new HashSet<Ingredient>(); //da si inicializiram vinagi ICollection-ite!!!!
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public TimeSpan? CookingTime { get; set; } //pozwolqwam da imam null za timespan

        [NotMapped]
        public string Test => this.CookingTime + " time is needed for cook.";
            //Test propertyto ne se vijda v DB-a, towa e ingored property. to e samo za moi nujdi i pomosht v classa.

        public ICollection<Ingredient> Ingredients { get; set; } //one-to-many relation

    }
}
