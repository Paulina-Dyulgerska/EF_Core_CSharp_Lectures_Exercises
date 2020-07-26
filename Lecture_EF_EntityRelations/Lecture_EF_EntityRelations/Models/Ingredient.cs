using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lecture_EF_EntityRelations.Models
{
    [Table("Ingredients")] //po princip imeto na tablicata se vzima ot imeto, koeto sym slojila na DbSet-a,
                           //no az moga da go sloja da e drugo i tuk
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }

        [Column("Title")] //na 3-to mqsto v tablicata sloji tazi colona
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; } //type int ne e int? i zatowa ne e nullable, t.e. avtomatichno stawa Requered!!!!
        //s RecipeId az dawam ime ne kolonata Id v class Recipe, za da si q polzwam posle s RecipeId mnogo priqtno 
        //za moi si nujdi!!!!

        [Required]
        public Recipe Recipe { get; set; } //tova property se zapisva v tablicata Ingredients kato nova colona, koqto
        //ima ime RecipeId i v neq se pazi Id-to na Recipeto, za koeto iskam da vyrja daden ingredient. Tova e
        //HAS-A relation. towa e one-to-many relation.
    }
}
