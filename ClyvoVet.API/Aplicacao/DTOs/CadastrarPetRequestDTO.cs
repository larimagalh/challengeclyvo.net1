using System.Globalization;

namespace ClyvoVet.API.Aplicação.DTOs;

public record CadastrarPetRequest(Guid IdPet, string nome , string Raca, double peso, string statusSaude);
