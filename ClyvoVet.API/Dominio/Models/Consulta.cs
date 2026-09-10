using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClyvoVet.API.Dominio.Models
{
    public class Consulta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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