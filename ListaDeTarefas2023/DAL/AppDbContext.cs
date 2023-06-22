using Microsoft.EntityFrameworkCore;
using ListaDeTarefas2023.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace ListaDeTarefas2023.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }
        public virtual DbSet<Tarefa>? Tarefas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.Property(x => x.Título).IsUnicode(false);
                entity.Property(x => x.Descrição).IsUnicode(false);
            });
        }
    }
}
