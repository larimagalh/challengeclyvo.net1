using System.ComponentModel.DataAnnotations;

namespace ClyvoVet.API.Models
{
    public class Consulta
    {
        [Key]
        public int IdConsulta { get; set; }

        public int IdPet { get; set; }

        public int IdResponsavel { get; set; }

        public DateTime DataConsulta { get; set; }

        public string TipoConsulta { get; set; }

        public string Sintomas { get; set; }

        public string Status { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.Now;
    }
}