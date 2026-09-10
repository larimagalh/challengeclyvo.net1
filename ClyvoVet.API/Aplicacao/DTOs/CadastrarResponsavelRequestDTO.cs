namespace ClyvoVet.API.Aplicação.DTOs;

public record CadastrarResponsavelRequest(Guid id, string Nome, string email, string telefone, string cidade, string estado);