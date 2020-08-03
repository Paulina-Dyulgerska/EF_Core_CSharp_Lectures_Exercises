using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShop.DataProcessor.ImportDto
{
    public class AuthorImportDTO
    {
        [Required, MinLength(3), MaxLength(30)]
        public string FirstName { get; set; }

        [Required, MinLength(3), MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        //Email - text(required). Validate it! There is attribute for this job.
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{4}$")]
        //Phone - text.Consists only of three groups(separated by '-'), the first two consist of three digits and the last one - of 4 digits. (required)
        public string Phone { get; set; }

        public BookIdDTO[] Books { get; set; }
    }
}
