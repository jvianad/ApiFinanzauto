using System.ComponentModel.DataAnnotations;

namespace ApiFinanzauto.Dto
{
    public class GradeDto
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public decimal GradeValue { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
