using System.ComponentModel.DataAnnotations;

namespace ApiFinanzauto.Dto
{
    public class StudentDto
    {
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
