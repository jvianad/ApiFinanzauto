using System.ComponentModel.DataAnnotations;

namespace ApiFinanzauto.Dto
{
    public class CourseDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "max 50 characters for names")]
        public string Name { get; set; }
        [Required]
        public int ProfessorId { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "max 500 characters for descriptions")]
        public string Description { get; set; }
    }
}
