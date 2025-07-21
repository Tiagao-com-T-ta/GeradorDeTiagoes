using GeradorDeTiagoes.Domain.DisciplineModule;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeradorDeTiagoes.Domain.Entities;

namespace GeradorDeTiagoes.Structure.Orm.SubjectModule
{
    class MapperSubject : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.Property(s => s.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.GradeLevel)
                            .IsRequired()
                            .HasMaxLength(100);

        }
    }
}
