namespace Lecture_EF_EntityRelations.Models
{
    public class StudentCourse
    {
        public int Id { get; set; } //da si ostavqm Id-to dori i da imam composite key, za da moga da si vkarwam
        //posle zapisi v tablicata bez da trqbwa da dawam id-ta az.

        public int StudentId { get; set; }
        
        public Student Student { get; set; }

        public int CourseId { get; set; }
        
        public Course Course { get; set; }
    }
}
