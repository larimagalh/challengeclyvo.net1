using ClyvoVet.API.Dominio.Models;

namespace ClyvoVet.API.Dominio.Interfaces
{
    public interface IResponsavelInterface
    {
        IEnumerable<Responsavel> GetAll();

        Responsavel? GetByID(Guid id);

        void Adicionar(Responsavel responsavel);

        void Deletar(Responsavel responsavel);
    }
}
