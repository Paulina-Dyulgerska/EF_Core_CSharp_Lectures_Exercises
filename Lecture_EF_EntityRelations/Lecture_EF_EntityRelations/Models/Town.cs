using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lecture_EF_EntityRelations.Models
{
    public class Town
    {
        public Town()
        {
            this.Workers = new HashSet<Employee>();
            this.Citizents = new HashSet<Employee>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        [InverseProperty("WorkTown")]
        public ICollection<Employee> Workers { get; set; } //v tazi collection sa vsichki Employess, koito
        //imat za WorkTown tozi Town.

        [InverseProperty("BirthTown")]
        public ICollection<Employee> Citizents { get; set; } //v tazi collection sa vsichki Employess, koito
        //imat za BirthTown tozi Town.
    }
}