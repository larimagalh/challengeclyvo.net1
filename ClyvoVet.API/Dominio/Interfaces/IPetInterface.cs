using ClyvoVet.API.Dominio.Models;

namespace ClyvoVet.API.Dominio.Interfaces
{
    public interface IPetInterface
    {
        IEnumerable<Pet> GetAll();

        Pet? GetByID(Guid id);

        void Adicionar(Pet pet);

        void Deletar(Consulta consulta);
    }
}
