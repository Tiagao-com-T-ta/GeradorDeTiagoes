using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.QuestionModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Structure.Orm.Shared
{
    public class GeradorDeTiagoesDbContext : DbContext
    {
        DbSet<Discipline> Disciplines { get; set; }
        DbSet<Subject> Subjects { get; set; }
        DbSet<Test> Tests { get; set; }
        DbSet<Question> Questions { get; set; }

        public GeradorDeTiagoesDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(GeradorDeTiagoesDbContext).Assembly;

            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);
        }
        
        
    }
}
