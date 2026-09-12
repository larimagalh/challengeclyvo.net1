using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace ClyvoVet.API.Infraestrutura.Observabilidade
{
    public static class AplicacaoMetricas
    {
            public const string NomeServico = "ClyvoVet.API";

            public static readonly Meter MeterAplicacao = new(NomeServico, "1.0.0");
            public static readonly ActivitySource ActivitySourceAplicacao = new(NomeServico);

        public static readonly Counter<long> ConsultasCriadasContador = MeterAplicacao.CreateCounter<long>(
        name: "Consultas_criadas_total",
        unit: "{Consultas}",
        description: "Contagem total de consultas cadastradas com sucesso na API");


        public static readonly Counter<long> PetsCriadosContador = MeterAplicacao.CreateCounter<long>(
        name: "Pets_criadas_total",
        unit: "{Pets}",
        description: "Contagem total de Pets cadastradas com sucesso na API");

        public static readonly Counter<long> ResponsaveisCriadosContador = MeterAplicacao.CreateCounter<long>(
        name: "responsaveis_criados_total",
        unit: "{Responsavel}",
        description: "Contagem total de responsáveis cadastrados na API");
    }
}

