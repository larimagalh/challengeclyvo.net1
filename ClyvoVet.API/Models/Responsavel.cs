using System.ComponentModel.DataAnnotations;

namespace ClyvoVet.API.Models
{
    public class Responsavel
    {
        [Key]
        public int IdResponsavel { get; set; }

        [Required]
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}