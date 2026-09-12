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
    public class ConsultasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ConsultasController> _logger;

        public ConsultasController(AppDbContext context, ILogger<ConsultasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consulta>>> Get()
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("ListarConsultasEndpoint");

            _logger.LogInformation("Iniciando listagem de consultas. ");

            return Ok(await _context.Consultas.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Consulta>> GetById(int id)
        {

            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("BuscarConsultaPorIdEndpoint");
            activity?.SetTag("consulta.id", id);

            _logger.LogInformation("Iniciando busca da consulta: {ConsultaId}", id);

            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null) {
                activity?.SetStatus(ActivityStatusCode.Error, "Consulta não encontrada");
                _logger.LogInformation("Consulta {ConsultaId} não encontrada", id);
                return NotFound();
            }
            return Ok(consulta);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Consulta>>> GetByStatus(string status)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("BuscarConsultasPorStatusEndpoint");
            activity?.SetTag("consulta.status", status);
            _logger.LogInformation("Iniciando busca da consulta: {Status}", status);

            var consultas = await _context.Consultas
                .Where(c => c.Status == status)
                .ToListAsync();

            return Ok(consultas);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Consulta consulta)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("CriarConsultaEndpoint");
            activity?.SetTag("consulta.IdConsulta", consulta.IdConsulta);
            activity?.SetTag("consulta.DataConsulta", consulta.DataConsulta);
            var id = consulta.IdConsulta;

            try
            {
                _logger.LogInformation("Adicionando nova consulta para o Pet {IdPet}", consulta.IdPet);

                _context.Consultas.Add(consulta);
                await _context.SaveChangesAsync();

                AplicacaoMetricas.ConsultasCriadasContador.Add(1,
                    new KeyValuePair<string, object?>("status", "sucesso"));

                activity?.SetTag("consulta.id", consulta.IdConsulta);
                _logger.LogInformation("Consulta {IdConsulta} cadastrada com sucesso", consulta.IdConsulta);

                return CreatedAtAction(nameof(GetById),
                    new { id = consulta.IdConsulta }, consulta);
            }
            catch (Exception ex)
            {
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                activity?.RecordException(ex);

                AplicacaoMetricas.ConsultasCriadasContador.Add(1,
                    new KeyValuePair<string, object?>("status", "erro"));

                _logger.LogError(ex, "Erro ao registrar consulta: {Mensagem}", ex.Message);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Consulta consulta)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("AtualizarConsultaEndpoint");
            activity?.SetTag("consulta.id", id);

            if (id != consulta.IdConsulta)
            {
                activity?.SetStatus(ActivityStatusCode.Error, "ID informado difere do payload");
                return BadRequest();
            }

            _context.Entry(consulta).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(consulta);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("DeletarConsultaEndpoint");
            activity?.SetTag("consulta.id", id);

            _logger.LogInformation("Deletando consulta: {idConsulta}", id);

            var consulta = await _context.Consultas.FindAsync(id);

            if (consulta == null) { 
            activity?.SetStatus(ActivityStatusCode.Error, "Consulta não encontrada para exclusão");
            _logger.LogWarning("Consulta {idConsulta} não encontrada", id);
            return NotFound();
            }
            _context.Consultas.Remove(consulta);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Consulta {idConsulta} deletada ", id);

            return Ok();
        }
    }
}