using ApiFinanzauto.Data;
using ApiFinanzauto.Interfaces;
using ApiFinanzauto.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;

namespace ApiFinanzauto.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IRepository<Student> _studentRepository;
        public StudentsController(IRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentRepository.GetAllAsync();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(students, options));
            
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStudent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(student, options));
            
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _studentRepository.PostAsync(student);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, JsonSerializer.Serialize(student, options));

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromBody] Student student)
        {
            if (id != student.Id || !ModelState.IsValid)
            {
                return BadRequest(new { message = $"The ID in the route ({id}) does not match the ID in the body ({student.Id})." });
            }

            var existingStudent = await _studentRepository.GetByIdAsync(id);
            if (existingStudent == null)
            {
                return NotFound();
            }
            existingStudent.Name = student.Name;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.Updated_At = DateTime.UtcNow;

            await _studentRepository.UpdateAsync(existingStudent);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent([FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _studentRepository.GetByIdAsync(id);
            if(student == null)
            {
                return NotFound();
            }

            await _studentRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
