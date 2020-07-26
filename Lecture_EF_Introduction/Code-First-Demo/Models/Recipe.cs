using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace Code_First_Demo.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } //NVARCHAR(MAX) e defaultnata stojnost, koqto shte se dade za string v C#
        public string Description { get; set; }
        public ICollection<Ingredient> Ingredients { get; set; }

    }
}
