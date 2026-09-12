using ClyvoVet.API.Dominio.Models;
using FluentAssertions;

namespace ClyvoVet.API.Tests.Unit.Dominio;

public class ConsultaTests
{
    [Fact]
    public void Construtor_DadosValidos_DeveCriarInstanciaComSucesso()
    {
        // Arrange (Preparação)
        int idPetValido = 1;
        int idResponsavelValido = 2;
        string sintomasValidos = "Febre e indisposição";
        string statusValido = "Agendada";
        DateTime dataValida = DateTime.Now.AddDays(1);

        // Act (Ação)
        var consulta = new Consulta
        {
            IdPet = idPetValido,
            IdResponsavel = idResponsavelValido,
            Sintomas = sintomasValidos,
            Status = statusValido,
            DataConsulta = dataValida
        };

        // Assert (Validação com FluentAssertions)
        consulta.Should().NotBeNull();
        consulta.IdPet.Should().Be(idPetValido);
        consulta.IdResponsavel.Should().Be(idResponsavelValido);
        consulta.Sintomas.Should().Be(sintomasValidos);
        consulta.Status.Should().Be(statusValido);
        consulta.DataConsulta.Should().Be(dataValida);
    }

}