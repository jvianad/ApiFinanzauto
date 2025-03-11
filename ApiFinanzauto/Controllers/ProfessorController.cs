using ApiFinanzauto.Interfaces;
using ApiFinanzauto.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;

namespace ApiFinanzauto.Controllers
{
    [Route("api/professor")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly IRepository<Professor> _professorRepository;

        public ProfessorController(IRepository<Professor> professorRepository)
        {
            _professorRepository = professorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProfessors()
        {
            var professors = await _professorRepository.GetAllAsync();
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return Ok(JsonSerializer.Serialize(professors, options));
        }

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

        [HttpPost]
        public async Task<IActionResult> CreateProfessor([FromBody] Professor professor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _professorRepository.PostAsync(professor);
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve, // Manejar referencias para evitar ciclos
                WriteIndented = true
            };
            return CreatedAtAction(nameof(GetProfessorById), new { id = professor.Id }, JsonSerializer.Serialize(professor, options));
            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProfessor([FromRoute] int id, [FromBody] Professor professor)
        {
            if (id != professor.Id || !ModelState.IsValid)
            {
                return BadRequest(new { message = $"The ID in the route ({id}) does not match the ID in the body ({professor.Id})." });
            }

            var existingProfessor = await _professorRepository.GetByIdAsync(id);
            if (existingProfessor == null)
            {
                return NotFound();
            }

            existingProfessor.FirstName = professor.FirstName;
            existingProfessor.LastName = professor.LastName;
            existingProfessor.Email = professor.Email;
            existingProfessor.Specialty = professor.Specialty;
            existingProfessor.Updated_At = DateTime.UtcNow;

            await _professorRepository.UpdateAsync(existingProfessor);
            return NoContent();
        }

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
