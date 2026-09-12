
using ClyvoVet.API.Dominio.Models;
using FluentAssertions;

namespace ClyvoVet.API.Tests.Unit.Dominio;

public class PetTests
{
    [Fact]
    public void Construtor_DadosValidos_DeveCriarInstanciaComSucesso()
    {
        // Arrange (Preparação)
        int idEsperado = 1;
        string nomeValido = "Rex";
        string especieValida = "Cachorro";

        // Act (Ação)
        var pet = new Pet
        {
            IdPet = idEsperado,
            Nome = nomeValido,
            Especie = especieValida
        };

        // Assert (Validação/Asserção)
        pet.Should().NotBeNull();
        pet.IdPet.Should().Be(idEsperado);
        pet.Nome.Should().Be(nomeValido);
        pet.Especie.Should().Be(especieValida);
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
                throw new ArgumentException("O nome do pet é obrigatório.");

            _ = new Pet { Nome = nomeInvalido };
        };

        // Assert
        acao.Should().Throw<ArgumentException>()
            .WithMessage("*O nome do pet é obrigatório.*");
    }
}