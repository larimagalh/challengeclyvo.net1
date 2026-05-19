using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClyvoVet.API.Data;
using ClyvoVet.API.Models;

namespace ClyvoVet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConsultasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consulta>>> Get()
        {
            return Ok(await _context.Consultas.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Consulta>> GetById(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null)
                return NotFound();

            return Ok(consulta);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Consulta>>> GetByStatus(string status)
        {
            var consultas = await _context.Consultas
                .Where(c => c.Status == status)
                .ToListAsync();

            return Ok(consultas);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Consulta consulta)
        {
            _context.Consultas.Add(consulta);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                new { id = consulta.IdConsulta }, consulta);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Consulta consulta)
        {
            if (id != consulta.IdConsulta)
                return BadRequest();

            _context.Entry(consulta).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(consulta);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null)
                return NotFound();

            _context.Consultas.Remove(consulta);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}