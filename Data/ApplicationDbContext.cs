using Microsoft.EntityFrameworkCore;
using CNIWebApp.Models;

namespace CNIWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Le constructeur transmet les options de connexion (MySQL) à la base
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet représente notre BD dans PhpMyAdmin.
        // C'est à travers cette propriété que le CRUD sera implementer sur la BD.
        public DbSet<Cni> cni_card { get; set; }

        // Cette méthode permet de configurer plus finement le comportement de la BD
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cni>().ToTable("cni_card");
        }
    }
}
