using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFinanzauto.Models
{
    public class Grade
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [Required]
        [Column(TypeName = "decimal(4,1)")]
        public decimal GradeValue { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime Created_At { get; set; }
        [Required]
        public DateTime Updated_At { get; set; }
        public Student? Student { get; set; }
        public Course? Course { get; set; }
    }
}
