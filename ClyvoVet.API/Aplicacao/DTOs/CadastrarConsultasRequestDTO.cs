namespace ClyvoVet.API.Aplicação.DTOs;

   public record CriarConsultaRequest(Guid ID, int IdPet, int IdResponsavel, DateTime DataConsulta, string TipoConsulta, string Sintomas, string Status);
