using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiFinanzauto.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(500, ErrorMessage = "max 500 characters for descriptions")]
        public string Description { get; set; }
        [Required]
        public DateTime Created_At { get; set; }
        [Required]
        public DateTime Updated_At { get; set; }
        [Required]
        [ForeignKey("Professor")]
        public int ProfessorId { get; set; }
        public Professor? Professor { get; set; }
        public List<Grade> Grades { get; set; } = new List<Grade>();
    }
}
