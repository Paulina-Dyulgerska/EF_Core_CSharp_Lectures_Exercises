using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Lecture_EF_EntityRelations.Models
{
	public class Address
	{
		public int Id { get; set; }

		public string Text { get; set; }

        public string Town { get; set; }

        [ForeignKey(nameof(Employee))]
		//[Required] //ako nqma required, shte e One-To-Zero, a az imam i bez towa required, zashtoto EmployeeId e
		//chist int, a ne e int?
		public int EmployeeId { get; set; }

		public Employee Employee { get; set; } //towa NE e collection, za da mi e to-one relationa.
	}
}
