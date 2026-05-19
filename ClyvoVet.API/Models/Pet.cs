using System.ComponentModel.DataAnnotations;

namespace ClyvoVet.API.Models
{
    public class Pet
    {
        [Key]
        public int IdPet { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Especie { get; set; }

        public string Raca { get; set; }

        public double Peso { get; set; }

        public string StatusSaude { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}