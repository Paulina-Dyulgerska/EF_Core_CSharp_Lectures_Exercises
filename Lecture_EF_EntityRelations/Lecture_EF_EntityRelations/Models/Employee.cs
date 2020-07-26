using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Lecture_EF_EntityRelations.Models
{
	public class Employee
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

        public int? BirthTownId { get; set; } //null za da ne mi gyrmi za delete

        public Town BirthTown { get; set; }

        public int? WorkTownId { get; set; } //null za da ne mi gyrmi za delete

		public Town WorkTown { get; set; }

		[ForeignKey("Address")]
		public int? AddressId { get; set; }
		//ako go ostavq int, a ne int? to EF Core gyrmi, zashtoto ne znae kakwo da pravi pri delete, ne
		//znae koj da iztrie pyrvi, ako ostawq i AddressId i EmployeeId da sa int samo!!!!!
		//za da ne mi gyrmi ili go pravq ednoto int?(EmployeeId ili AddressId) ili moga da gi nastroq prez Fluent API i tam stawa wsichko:            
		//modelBuilder.Entity<Employee>()
		//    .HasOne(x => x.Address)
		//    .WithOne(x => x.Employee)
		//    .OnDelete(DeleteBehavior.Restrict);
		public Address Address { get; set; } //dostatychno e samo towa, bez gorniq red, no e po-dobre da imam i dvata reda!!!!
											 //towa NE e collection, za da mi e to-one relationa.
	}
	
}
