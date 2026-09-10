using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ClyvoVet.API.Infraestrutura.Health
{
    public class BancoDadosHealthCheck : IHealthCheck
    {
        private readonly Random _random = new();
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            // Simula a verificação de latência/conexão com o banco de dados
            bool conexaoOk = _random.Next(1, 10) > 1; // 90% de chance de sucesso

            if (conexaoOk)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("Conexão com o Banco de Dados estabelecida com sucesso.",
                    data: new Dictionary<string, object> { { "LatenciaMs", 12 } }));
            }

            return Task.FromResult(
                HealthCheckResult.Unhealthy("Falha ao conectar no Banco de Dados (Oracle/MongoDB)."));
        }
    }

}