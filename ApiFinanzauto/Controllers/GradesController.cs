using ApiFinanzauto.Interfaces;
using ApiFinanzauto.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using ApiFinanzauto.Dto;
using ApiFinanzauto.Filters;

namespace ApiFinanzauto.Controllers
{
    [Route("api/grades")]
    [ApiController]
    public class GradesController:ControllerBase
    {
        private readonly IRepository<Grade> _gradeRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Course> _courseRepository;

        public GradesController(IRepository<Grade> gradeRepository, IRepository<Student> studentRepository, IRepository<Course> courseRepository)
        {
            _gradeRepository = gradeRepository;
            _studentRepository = studentRepository;
            _courseRepository = courseRepository;

        }
        /// <summary>
        /// Obtiene una lista de todas las calificaciones con filtros opcionales.
        /// </summary>
        /// <param name="studentId">Filtro por ID del estudiante (opcional).</param>
        /// <param name="courseId">Filtro por ID del curso (opcional).</param>
        /// <param name="gradeValueMin">Filtro por valor mínimo de la calificación (opcional).</param>
        /// <param name="gradeValueMax">Filtro por valor máximo de la calificación (opcional).</param>
        /// <returns>Una lista de calificaciones que coinciden con los filtros.</returns>
        /// <response code="200">Devuelve la lista de calificaciones.</response>
        [HttpGet]
        public async Task<IActionResult> GetGrades([FromQuery] int? studentId = null,
            [FromQuery] int? courseId = null,
            [FromQuery] decimal? gradeValueMin = null,
            [FromQuery] decimal? gradeValueMax = null)
        {
            var filter = new GradeFilter
            {
                StudentId = studentId,
                CourseId = courseId,
                GradeValueMin = gradeValueMin,
                GradeValueMax = gradeValueMax
            };

            var grades = await _gradeRepository.GetAllAsync(filter);
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(grades, options));
        }

        /// <summary>
        /// Obtiene una calificación específica por su ID.
        /// </summary>
        /// <param name="id">El ID de la calificación a buscar.</param>
        /// <returns>La calificación con el ID especificado.</returns>
        /// <response code="200">Devuelve la calificación encontrada.</response>
        /// <response code="404">Si la calificación no se encuentra.</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGrade([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var grade = await _gradeRepository.GetByIdAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(grade, options));
            
        }

        /// <summary>
        /// Crea una nueva calificación asociada a un estudiante y curso.
        /// </summary>
        /// <param name="gradeDto">Datos de la calificación a crear.</param>
        /// <returns>La calificación creada con ID asignado.</returns>
        /// <response code="201">Devuelve la calificación creada.</response>
        /// <response code="400">Si los datos de entrada son inválidos.</response>
        [HttpPost]
        public async Task<IActionResult> CreateGrade([FromBody] GradeDto gradeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await _studentRepository.GetByIdAsync(gradeDto.StudentId);
            if (student == null)
            {
                return BadRequest(new { message = $"Student with ID {gradeDto.StudentId} does not exist." });
            }

            var course = await _courseRepository.GetByIdAsync(gradeDto.CourseId);
            if (course == null)
            {
                return BadRequest(new { message = $"Course with ID {gradeDto.CourseId} does not exist." });
            }

            var grade = new Grade
            {
                CourseId = gradeDto.CourseId,
                StudentId = gradeDto.StudentId,
                GradeValue = gradeDto.GradeValue,
                Date = gradeDto.Date
            };

            await _gradeRepository.PostAsync(grade);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return CreatedAtAction(nameof(GetGrade), new { id = grade.Id }, JsonSerializer.Serialize(grade, options));
            
        }

        /// <summary>
        /// Actualiza una calificación existente.
        /// </summary>
        /// <param name="id">El ID de la calificación a actualizar.</param>
        /// <param name="gradeDto">Datos actualizados de la calificación.</param>
        /// <returns>No Content si la actualización fue exitosa.</returns>
        /// <response code="204">Si la calificación se actualizó correctamente.</response>
        /// <response code="400">Si los datos de entrada son inválidos.</response>
        /// <response code="404">Si la calificación no se encuentra.</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateGrade([FromRoute] int id, [FromBody] GradeDto gradeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existingGrade = await _gradeRepository.GetByIdAsync(id);
            if (existingGrade == null)
            {
                return NotFound();
            }

            var student = await _studentRepository.GetByIdAsync(gradeDto.StudentId);
            if (student == null)
            {
                return BadRequest(new { message = $"Student with ID {gradeDto.StudentId} does not exist." });
            }

            var course = await _courseRepository.GetByIdAsync(gradeDto.CourseId);
            if (course == null)
            {
                return BadRequest(new { message = $"Course with ID {gradeDto.CourseId} does not exist." });
            }

            existingGrade.StudentId = gradeDto.StudentId;
            existingGrade.CourseId = gradeDto.CourseId;
            existingGrade.GradeValue = gradeDto.GradeValue;
            existingGrade.Date = gradeDto.Date;
            existingGrade.Updated_At = DateTime.UtcNow;

            await _gradeRepository.UpdateAsync(existingGrade);
            return NoContent();
        }

        /// <summary>
        /// Elimina una calificación por su ID.
        /// </summary>
        /// <param name="id">El ID de la calificación a eliminar.</param>
        /// <returns>No Content si la eliminación fue exitosa.</returns>
        /// <response code="204">Si la calificación se eliminó correctamente.</response>
        /// <response code="404">Si la calificación no se encuentra.</response>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGrade([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var grade = await _gradeRepository.GetByIdAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            await _gradeRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
