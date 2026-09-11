using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClyvoVet.API.Data;
using ClyvoVet.API.Dominio.Models;

namespace ClyvoVet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PetsController> _logger;

        public PetsController(AppDbContext context, ILogger<PetsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pet>>> Get()
        {
            _logger.LogInformation("Listando todos os Pets no sistema");
            return Ok(await _context.Pets.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pet>> GetById(int id)
        {
            _logger.LogInformation("Procurando por Pet: {petID}", id);
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null) { 

            _logger.LogInformation("Pet nao encontrado");
            return NotFound($"Pet {id}, não encontrado");
            
            }
            
        return Ok(pet);
        }

        [HttpGet("especie/{especie}")]
        public async Task<ActionResult<IEnumerable<Pet>>> GetByEspecie(string especie)
        {
            var pets = await _context.Pets
                .Where(p => p.Especie == especie)
                .ToListAsync();

            return Ok(pets);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Pet pet)
        {
            _context.Pets.Add(pet);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),
                new { id = pet.IdPet }, pet);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Pet pet)
        {
            if (id != pet.IdPet)
                return BadRequest();

            _context.Entry(pet).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(pet);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
                return NotFound();

            _context.Pets.Remove(pet);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}