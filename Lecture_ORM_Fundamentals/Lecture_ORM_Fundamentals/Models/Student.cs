using System.Collections.Generic;

namespace Lecture_ORM_Fundamentals.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Grade> Grades { get; set; } //tova property ne otide kato colona v tablica Students v DB-a!!!

    }
}
