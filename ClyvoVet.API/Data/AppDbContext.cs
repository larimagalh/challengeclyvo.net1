using Microsoft.EntityFrameworkCore;
using ClyvoVet.API.Models;

namespace ClyvoVet.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<Responsavel> Responsaveis { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
    }
}