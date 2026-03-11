using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Data {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Configuración adicional si la necesitas (ej. nombres de tabla)
            modelBuilder.Entity<TaskItem>().ToTable("Tasks");
        }
    }
}