using System.Collections.Generic;

namespace Lecture_EF_EntityRelations.Models
{
    public class Course
    {
        public Course()
        {
            this.Students = new HashSet<StudentCourse>(); //da si inicializiram vinagi ICollection-ite!!!!
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<StudentCourse> Students { get; set; }
    }
}
