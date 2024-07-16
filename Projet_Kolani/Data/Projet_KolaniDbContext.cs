using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Projet_Kolani.Models;

namespace Projet_Kolani.Data
{
    public class Projet_KolaniDbContext : IdentityDbContext<IdentityUser>


    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ici, vous devez fournir la chaîne de connexion à votre base de données SQL Server
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=FlorentDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        public DbSet<Proprietaire> Proprietaires { get; set; }
        public DbSet<Engin> Engins { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<Reglement> Reglements { get; set; }
        public object Users { get; internal set; }
    }
}
