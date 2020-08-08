﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class User
    {
        public User()
        {
            this.Cards = new HashSet<Card>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int Age { get; set; }

        public virtual ICollection<Card> Cards { get; set; }

        //Username – text with length[3, 20] (required)
        //FullName – text, which has two words, consisting of Latin letters.
        ////Both start with an upper letter and are followed by lower letters.
        ////The two words are separated by a single space (ex. "John Smith") (required)
        //Email – text(required)
        //Age – integer in the range[3, 103] (required)
        //Cards – collection of type Card
    }
}
