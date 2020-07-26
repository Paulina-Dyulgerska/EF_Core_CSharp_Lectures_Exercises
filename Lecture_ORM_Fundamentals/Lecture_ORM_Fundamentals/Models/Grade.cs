namespace Lecture_ORM_Fundamentals.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public decimal GradeValue { get; set; }
        public Student Student { get; set; } //tova e navigational property
        public Course Course { get; set; } //tova e navigational property

    }
}
