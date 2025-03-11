using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiFinanzauto.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "max 50 characters for names")]
        public string Name { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "max 50 characters for last names")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public DateTime Created_At { get; set; }
        [Required]
        public DateTime Updated_At { get; set; }

        //1 student has many grades
        public List<Grade> Grades { get; set; } = new List<Grade>();
    }
}
