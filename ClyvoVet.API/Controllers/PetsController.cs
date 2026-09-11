using ClyvoVet.API.Data;
using ClyvoVet.API.Dominio.Models;
using ClyvoVet.API.Infraestrutura.Observabilidade;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using System.Diagnostics;

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
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("ListarPetsEndpoint");

            _logger.LogInformation("Listando todos os Pets no sistema");
            return Ok(await _context.Pets.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pet>> GetById(int id)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("BuscarPetPorIdEndpoint");
            activity?.SetTag("pet.id", id);

            _logger.LogInformation("Procurando por Pet: {petID}", id);
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
            {

                activity?.SetStatus(ActivityStatusCode.Error, "Pet não encontrado");
                _logger.LogInformation("Pet nao encontrado");
                return NotFound($"Pet {id}, não encontrado");

            }

            return Ok(pet);
        }

        [HttpGet("especie/{especie}")]
        public async Task<ActionResult<IEnumerable<Pet>>> GetByEspecie(string especie)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("BuscarPetsPorEspecieEndpoint");
            activity?.SetTag("pet.especie", especie);

            _logger.LogInformation("Procurando por especie {nomeEsp}", especie);
            var pets = await _context.Pets
                .Where(p => p.Especie == especie)
                .ToListAsync();

            return Ok(pets);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Pet pet)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("CadastrarPetEndpoint");
            activity?.SetTag("pet.nome", pet.Nome);
            activity?.SetTag("pet.especie", pet.Especie);

            try
            {
                _logger.LogInformation("Adicionando novo Pet {idPet}", pet.IdPet);
                _context.Pets.Add(pet);

                _logger.LogInformation("Salvando o {nomePet} no banco", pet.Nome);
                await _context.SaveChangesAsync();

                AplicacaoMetricas.PetsCriadosContador.Add(1, new KeyValuePair<string, object?>("status", "sucesso"));

                return CreatedAtAction(nameof(GetById),
                    new { id = pet.IdPet }, pet);
            }
            catch (Exception ex)
            {
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                activity?.RecordException(ex);
                AplicacaoMetricas.PetsCriadosContador.Add(1, new KeyValuePair<string, object?>("status", "erro"));
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Pet pet)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("AtualizarPetEndpoint");
            activity?.SetTag("pet.id", id);

            if (id != pet.IdPet)
            {
                activity?.SetStatus(ActivityStatusCode.Error, "ID incompatível");
                return BadRequest();
            }

            _context.Entry(pet).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(pet);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("DeletarPetEndpoint");
            activity?.SetTag("pet.id", id);

            _logger.LogInformation("Deletando o Pet {idPet}", id);
            var pet = await _context.Pets.FindAsync(id);

            if (pet == null)
            {
                activity?.SetStatus(ActivityStatusCode.Error, "Pet não encontrado");
                _logger.LogInformation("Pet {idPet} não encontrado", id);
                return NotFound();
            }

            _logger.LogInformation("Pet {idPet} deletado", id);
            _context.Pets.Remove(pet);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}