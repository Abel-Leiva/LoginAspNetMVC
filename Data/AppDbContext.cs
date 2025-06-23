using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PracticaLogin.Data;
using PracticaLogin.Models;
namespace PracticaLogin.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>(tb =>
{
    tb.HasKey(col => col.IdUsuario);
    tb.Property(col => col.IdUsuario).UseIdentityColumn().ValueGeneratedOnAdd();

    tb.Property(col => col.NombreCompleto).HasMaxLength(50);
    tb.Property(col => col.Correo).HasMaxLength(50);
    tb.Property(col => col.Clave).HasMaxLength(150);
    tb.HasIndex(col => col.Correo).IsUnique();

});
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
        }
    }
}