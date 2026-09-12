using ClyvoVet.API.Dominio.Models;

namespace ClyvoVet.API.Dominio.Interfaces
{
    public interface IConsultaInterface
    {
        IEnumerable<Consulta> GetAll();

        Consulta? GetByID(Guid id);

        void Adicionar(Consulta consulta);

        void Deletar(Consulta consulta);
    }
}
