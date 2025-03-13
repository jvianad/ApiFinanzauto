using ApiFinanzauto.Data;
using ApiFinanzauto.Interfaces;
using ApiFinanzauto.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;
using ApiFinanzauto.Filters;
using ApiFinanzauto.Dto;
using Microsoft.AspNetCore.Authorization;

namespace ApiFinanzauto.Controllers
{
    [Route("api/student")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IRepository<Student> _studentRepository;
        public StudentsController(IRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        /// <summary>
        /// Obtiene una lista de todos los estudiantes con filtros opcionales.
        /// </summary>
        /// <param name="firstName">Filtro por nombre del estudiante (opcional).</param>
        /// <param name="lastName">Filtro por apellido del estudiante (opcional).</param>
        /// <param name="email">Filtro por email del estudiante (opcional).</param>
        /// <returns>Una lista de estudiantes que coinciden con los filtros.</returns>
        /// /// <response code="200">Devuelve la lista de estudiantes.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllStudents([FromQuery] string firstName = null,
            [FromQuery] string lastName = null,
            [FromQuery] string email = null)
        {
            var filter = new StudentFilter
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            var students = await _studentRepository.GetAllAsync(filter);
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(students, options));

        }

        /// <summary>
        /// Obtiene un estudiante específico por su ID.
        /// </summary>
        /// <param name="id">El ID del estudiante a buscar.</param>
        /// <returns>El estudiante con el ID especificado.</returns>
        /// <response code="200">Devuelve el estudiante encontrado.</response>
        /// <response code="404">Si el estudiante no se encuentra.</response>
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

        /// <summary>
        /// Crea un nuevo estudiante.
        /// </summary>
        /// <param name="studentDto">Datos del estudiante a crear.</param>
        /// <returns>El estudiante creado con ID asignado.</returns>
        /// <response code="201">Devuelve el estudiante creado.</response>
        /// <response code="400">Si los datos de entrada son inválidos.</response>
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = new Student
            {
                Name = studentDto.Name,
                LastName = studentDto.LastName,
                Email = studentDto.Email,
                Created_At = DateTime.UtcNow,
                Updated_At = DateTime.UtcNow
            };

            await _studentRepository.PostAsync(student);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, JsonSerializer.Serialize(student, options));

        }

        /// <summary>
        /// Actualiza un estudiante existente.
        /// </summary>
        /// <param name="id">El ID del estudiante a actualizar.</param>
        /// <param name="studentDto">Datos actualizados del estudiante.</param>
        /// <returns>No Content si la actualización fue exitosa.</returns>
        /// <response code="204">Si el estudiante se actualizó correctamente.</response>
        /// <response code="400">Si los datos de entrada son inválidos.</response>
        /// <response code="404">Si el estudiante no se encuentra.</response>
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] int id, [FromBody] StudentDto studentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingStudent = await _studentRepository.GetByIdAsync(id);
            if (existingStudent == null)
            {
                return NotFound();
            }
            existingStudent.Name = studentDto.Name;
            existingStudent.LastName = studentDto.LastName;
            existingStudent.Email = studentDto.Email;
            existingStudent.Updated_At = DateTime.UtcNow;

            await _studentRepository.UpdateAsync(existingStudent);
            return NoContent();
        }
        /// <summary>
        /// Elimina un estudiante por su ID.
        /// </summary>
        /// <param name="id">El ID del estudiante a eliminar.</param>
        /// <returns>No Content si la eliminación fue exitosa.</returns>
        /// <response code="204">Si el estudiante se eliminó correctamente.</response>
        /// <response code="404">Si el estudiante no se encuentra.</response>
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
