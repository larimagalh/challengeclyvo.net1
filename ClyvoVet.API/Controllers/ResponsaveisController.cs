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
    public class ResponsaveisController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ResponsaveisController> _logger;

        public ResponsaveisController(AppDbContext context, ILogger<ResponsaveisController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Responsavel>>> Get()
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("ListarResponsaveisEndpoint");

            _logger.LogInformation("Listando todos os responsáveis cadastrados.");
            return Ok(await _context.Responsaveis.AsNoTracking().ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Responsavel>> GetById(int id)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("BuscarResponsavelPorIdEndpoint");
            activity?.SetTag("responsavel.id", id);

            _logger.LogInformation("Buscando responsável pelo ID: {ResponsavelId}", id);
            var responsavel = await _context.Responsaveis.FindAsync(id);

            if (responsavel == null)
            {
                activity?.SetStatus(ActivityStatusCode.Error, "Responsável não encontrado");
                _logger.LogWarning("Responsável com ID {ResponsavelId} não foi encontrado.", id);
                return NotFound();
            }

            return Ok(responsavel);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Responsavel responsavel)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("CadastrarResponsavelEndpoint");
            activity?.SetTag("responsavel.nome", responsavel.Nome);

            try
            {
                _logger.LogInformation("Cadastrando novo responsável: {NomeResponsavel}", responsavel.Nome);

                _context.Responsaveis.Add(responsavel);
                await _context.SaveChangesAsync();

                AplicacaoMetricas.ResponsaveisCriadosContador.Add(1,
                    new KeyValuePair<string, object?>("status", "sucesso"));

                activity?.SetTag("responsavel.id", responsavel.IdResponsavel);
                _logger.LogInformation("Responsável cadastrado com sucesso. ID: {ResponsavelId}", responsavel.IdResponsavel);

                return CreatedAtAction(nameof(GetById),
                    new { id = responsavel.IdResponsavel }, responsavel);
            }
            catch (Exception ex)
            {
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                activity?.RecordException(ex);

                AplicacaoMetricas.ResponsaveisCriadosContador.Add(1,
                    new KeyValuePair<string, object?>("status", "erro"));

                _logger.LogError(ex, "Erro ao registrar responsável: {Mensagem}", ex.Message);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Responsavel responsavel)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("AtualizarResponsavelEndpoint");
            activity?.SetTag("responsavel.id", id);

            if (id != responsavel.IdResponsavel)
            {
                activity?.SetStatus(ActivityStatusCode.Error, "ID divergente");
                _logger.LogWarning("ID da rota ({RotaId}) difere do payload ({PayloadId}).", id, responsavel.IdResponsavel);
                return BadRequest();
            }

            _context.Entry(responsavel).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Responsável {ResponsavelId} atualizado com sucesso.", id);
            return Ok(responsavel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            using var activity = AplicacaoMetricas.ActivitySourceAplicacao.StartActivity("DeletarResponsavelEndpoint");
            activity?.SetTag("responsavel.id", id);

            _logger.LogInformation("Deletando responsável com ID: {ResponsavelId}", id);
            var responsavel = await _context.Responsaveis.FindAsync(id);

            if (responsavel == null)
            {
                activity?.SetStatus(ActivityStatusCode.Error, "Responsável não encontrado");
                _logger.LogWarning("Responsável {ResponsavelId} não encontrado para exclusão.", id);
                return NotFound();
            }

            _context.Responsaveis.Remove(responsavel);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Responsável {ResponsavelId} removido.", id);
            return NoContent();
        }
    }
}