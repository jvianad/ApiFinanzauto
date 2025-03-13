using ApiFinanzauto.Dto;
using ApiFinanzauto.Interfaces;
using ApiFinanzauto.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using ApiFinanzauto.Filters;
using Microsoft.AspNetCore.Authorization;

namespace ApiFinanzauto.Controllers
{
    [Route("api/course")]
    [ApiController]
    [Authorize]
    public class CourseController:ControllerBase
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Professor> _professorRepository;

        public CourseController(IRepository<Course> courseRepository, IRepository<Professor> professorRepository)
        {
            _courseRepository = courseRepository;
            _professorRepository = professorRepository;
        }

        /// <summary>
        /// Obtiene una lista de todos los cursos con filtros opcionales.
        /// </summary>
        /// <param name="name">Filtro por nombre del curso (opcional).</param>
        /// <param name="professorId">Filtro por ID del profesor (opcional).</param>
        /// <returns>Una lista de cursos que coinciden con los filtros.</returns>
        /// <response code="200">Devuelve la lista de cursos.</response>
        [HttpGet]
        public async Task<IActionResult> GetCourses([FromQuery] string name = null,
            [FromQuery] int? professorId = null)
        {
            var filter = new CourseFilter
            {
                Name = name,
                ProfessorId = professorId
            };

            var courses = await _courseRepository.GetAllAsync(filter);
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(courses, options));
            
        }

        /// <summary>
        /// Obtiene un curso específico por su ID.
        /// </summary>
        /// <param name="id">El ID del curso a buscar.</param>
        /// <returns>El curso con el ID especificado.</returns>
        /// <response code="200">Devuelve el curso encontrado.</response>
        /// <response code="404">Si el curso no se encuentra.</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCourse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(course, options));
        }

        /// <summary>
        /// Crea un nuevo curso.
        /// </summary>
        /// <param name="courseDto">Datos del curso a crear.</param>
        /// <returns>El curso creado con ID asignado.</returns>
        /// <response code="201">Devuelve el curso creado.</response>
        /// <response code="400">Si los datos de entrada son inválidos.</response>
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseDto courseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (courseDto.ProfessorId != 0)
            {
                var professor = await _professorRepository.GetByIdAsync(courseDto.ProfessorId);
                if(professor == null)
                {
                    return BadRequest(new { message = $"Professor with ID {courseDto.ProfessorId} does not exist." });
                }
            }

            var course = new Course
            {
                Name = courseDto.Name,
                Description = courseDto.Description,
                ProfessorId = courseDto.ProfessorId,
                Created_At = DateTime.UtcNow,
                Updated_At = DateTime.UtcNow
            };

            await _courseRepository.PostAsync(course);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, JsonSerializer.Serialize(course, options));
        }

        /// <summary>
        /// Actualiza un curso existente.
        /// </summary>
        /// <param name="id">El ID del curso a actualizar.</param>
        /// <param name="courseDto">Datos actualizados del curso.</param>
        /// <returns>No Content si la actualización fue exitosa.</returns>
        /// <response code="204">Si el curso se actualizó correctamente.</response>
        /// <response code="400">Si los datos de entrada son inválidos.</response>
        /// <response code="404">Si el curso no se encuentra.</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCourse([FromRoute] int id, [FromBody] CourseDto courseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCourse = await _courseRepository.GetByIdAsync(id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            if (courseDto.ProfessorId != 0)
            {
                var professor = await _professorRepository.GetByIdAsync(courseDto.ProfessorId);
                if (professor == null)
                {
                    return BadRequest(new { message = $"Professor with ID {courseDto.ProfessorId} does not exist." });
                }
            }

            existingCourse.Name = courseDto.Name;
            existingCourse.Description = courseDto.Description;
            existingCourse.ProfessorId = courseDto.ProfessorId;
            existingCourse.Updated_At = DateTime.UtcNow;

            await _courseRepository.UpdateAsync(existingCourse);
            return NoContent();
        }

        /// <summary>
        /// Elimina un curso por su ID.
        /// </summary>
        /// <param name="id">El ID del curso a eliminar.</param>
        /// <returns>No Content si la eliminación fue exitosa.</returns>
        /// <response code="204">Si el curso se eliminó correctamente.</response>
        /// <response code="404">Si el curso no se encuentra.</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCourse([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            await _courseRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
