using P03_FootballBetting.Data.Models.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace P03_FootballBetting.Data.Models
{
   public class Bet
    {
        public int BetId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public Prediction Prediction { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

        [Required]
        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}
