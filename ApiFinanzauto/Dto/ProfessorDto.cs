using System.ComponentModel.DataAnnotations;

namespace ApiFinanzauto.Dto
{
    public class ProfessorDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "max 50 characters for names")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "max 50 characters for last names")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Specialty { get; set; }
    }
}
