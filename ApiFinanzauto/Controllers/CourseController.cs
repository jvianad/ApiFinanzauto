using ApiFinanzauto.Dto;
using ApiFinanzauto.Interfaces;
using ApiFinanzauto.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using ApiFinanzauto.Filters;

namespace ApiFinanzauto.Controllers
{
    [Route("api/course")]
    [ApiController]
    public class CourseController:ControllerBase
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Professor> _professorRepository;

        public CourseController(IRepository<Course> courseRepository, IRepository<Professor> professorRepository)
        {
            _courseRepository = courseRepository;
            _professorRepository = professorRepository;
        }

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
