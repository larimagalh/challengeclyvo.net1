using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClyvoVet.API.Data;
using ClyvoVet.API.Models;

namespace ClyvoVet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponsaveisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResponsaveisController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Responsavel>>> Get()
        {
            return Ok(await _context.Responsaveis.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Responsavel>> GetById(int id)
        {
            var responsavel = await _context.Responsaveis.FindAsync(id);

            if (responsavel == null)
                return NotFound();

            return Ok(responsavel);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Responsavel responsavel)
        {
            _context.Responsaveis.Add(responsavel);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                new { id = responsavel.IdResponsavel }, responsavel);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Responsavel responsavel)
        {
            if (id != responsavel.IdResponsavel)
                return BadRequest();

            _context.Entry(responsavel).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(responsavel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var responsavel = await _context.Responsaveis.FindAsync(id);

            if (responsavel == null)
                return NotFound();

            _context.Responsaveis.Remove(responsavel);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}