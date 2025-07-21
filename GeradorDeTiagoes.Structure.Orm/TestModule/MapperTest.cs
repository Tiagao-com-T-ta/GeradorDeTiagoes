using GeradorDeTiagoes.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Structure.Orm.TestModule
{
    class MapperTest : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.Property(t => t.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(t => t.Title)
                .IsRequired();

            builder.Property(t => t.GradeLevel)
                            .IsRequired();

            builder.Property(t => t.QuestionCount)
                            .IsRequired();

            builder.Property(t => t.IsRecovery)
                           .IsRequired();

            builder.Property(t => t.DisciplineId)
                          .IsRequired();

            builder.HasOne(t => t.Discipline)
                          .WithMany()
                          .HasForeignKey(t => t.DisciplineId);

            builder.HasOne(t => t.Subject)
                          .WithMany()
                          .HasForeignKey(t => t.SubjectId);

        }
    }
}
