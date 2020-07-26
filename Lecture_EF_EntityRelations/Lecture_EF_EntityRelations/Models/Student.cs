using System.Collections.Generic;

namespace Lecture_EF_EntityRelations.Models
{
    public class Student
    {
        public Student()
        {
            this.Courses = new HashSet<StudentCourse>(); //da si inicializiram vinagi ICollection-ite!!!!
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<StudentCourse> Courses { get; set; }
    }
}
