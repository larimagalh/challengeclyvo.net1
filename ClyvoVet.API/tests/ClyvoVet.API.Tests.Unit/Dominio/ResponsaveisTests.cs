using ClyvoVet.API.Dominio.Models;
using FluentAssertions;

namespace ClyvoVet.API.Tests.Unit.Dominio;

public class ResponsavelTests
{
    [Fact]
    public void Construtor_DadosValidos_DeveCriarInstanciaComSucesso()
    {
        // Arrange (Preparação)
        int idEsperado = 1;
        string nomeValido = "Carlos Eduardo";

        // Act (Ação)
        var responsavel = new Responsavel
        {
            IdResponsavel = idEsperado,
            Nome = nomeValido
        };

        // Assert (Validação/Asserção)
        responsavel.Should().NotBeNull();
        responsavel.IdResponsavel.Should().Be(idEsperado);
        responsavel.Nome.Should().Be(nomeValido);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validar_NomeInvalido_DeveLancarArgumentException(string? nomeInvalido)
    {
        // Arrange & Act
        Action acao = () =>
        {
            if (string.IsNullOrWhiteSpace(nomeInvalido))
                throw new ArgumentException("O nome do responsável é obrigatório.");

            _ = new Responsavel { Nome = nomeInvalido };
        };

        // Assert
        acao.Should().Throw<ArgumentException>()
            .WithMessage("*O nome do responsável é obrigatório.*");
    }
}

