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
    [Route("api/professor")]
    [ApiController]
    [Authorize]
    public class ProfessorController : ControllerBase
    {
        private readonly IRepository<Professor> _professorRepository;

        public ProfessorController(IRepository<Professor> professorRepository)
        {
            _professorRepository = professorRepository;
        }

        /// <summary>
        /// Obtiene una lista de todos los profesores con filtros opcionales.
        /// </summary>
        /// <param name="firstName">Filtro por nombre del profesor (opcional).</param>
        /// <param name="lastName">Filtro por apellido del profesor (opcional).</param>
        /// <param name="email">Filtro por email del profesor (opcional).</param>
        /// <returns>Una lista de profesores que coinciden con los filtros.</returns>
        /// /// <response code="200">Devuelve la lista de profesores.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllProfessors([FromQuery] string firstName = null,
            [FromQuery] string lastName = null,
            [FromQuery] string email = null)
        {
            var filter = new ProfessorFilter
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            var professors = await _professorRepository.GetAllAsync(filter);
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(professors, options));
        }

        /// <summary>
        /// Obtiene un profesor específico por su ID.
        /// </summary>
        /// <param name="id">El ID del profesor a buscar.</param>
        /// <returns>El profesor con el ID especificado.</returns>
        /// <response code="200">Devuelve el profesor encontrado.</response>
        /// <response code="404">Si el profesor no se encuentra.</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProfessorById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var professor = await _professorRepository.GetByIdAsync(id);
            if (professor == null)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(professor, options));


          
        }

        /// <summary>
        /// Crea un nuevo profesor.
        /// </summary>
        /// <param name="professorDto">Datos del profesor a crear.</param>
        /// <returns>El profesor creado con ID asignado.</returns>
        /// <response code="201">Devuelve el profesor creado.</response>
        /// <response code="400">Si los datos de entrada son inválidos.</response>
        [HttpPost]
        public async Task<IActionResult> CreateProfessor([FromBody] ProfessorDto professorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var professor = new Professor
            {
                FirstName = professorDto.FirstName,
                LastName = professorDto.LastName,
                Email = professorDto.Email,
                Specialty = professorDto.Specialty,
                Created_At = DateTime.UtcNow,
                Updated_At = DateTime.UtcNow
            };

            await _professorRepository.PostAsync(professor);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return CreatedAtAction(nameof(GetProfessorById), new { id = professor.Id }, JsonSerializer.Serialize(professor, options));
            
        }

        /// <summary>
        /// Actualiza un profesor existente.
        /// </summary>
        /// <param name="id">El ID del profesor a actualizar.</param>
        /// <param name="professorDto">Datos actualizados del profesor.</param>
        /// <returns>No Content si la actualización fue exitosa.</returns>
        /// <response code="204">Si el profesor se actualizó correctamente.</response>
        /// <response code="400">Si los datos de entrada son inválidos.</response>
        /// <response code="404">Si el profesor no se encuentra.</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProfessor([FromRoute] int id, [FromBody] ProfessorDto professorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingProfessor = await _professorRepository.GetByIdAsync(id);
            if (existingProfessor == null)
            {
                return NotFound();
            }

            existingProfessor.FirstName = professorDto.FirstName;
            existingProfessor.LastName = professorDto.LastName;
            existingProfessor.Email = professorDto.Email;
            existingProfessor.Specialty = professorDto.Specialty;
            existingProfessor.Updated_At = DateTime.UtcNow;

            await _professorRepository.UpdateAsync(existingProfessor);
            return NoContent();
        }

        /// <summary>
        /// Elimina un profesor por su ID.
        /// </summary>
        /// <param name="id">El ID del profesor a eliminar.</param>
        /// <returns>No Content si la eliminación fue exitosa.</returns>
        /// <response code="204">Si el profesor se eliminó correctamente.</response>
        /// <response code="404">Si el profesor no se encuentra.</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProfessor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var professor = await _professorRepository.GetByIdAsync(id);
            if (professor == null)
            {
                return NotFound();
            }

            await _professorRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
