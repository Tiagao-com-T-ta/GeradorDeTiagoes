using GeradorDeTiagoes.Domain.QuestionModule;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Structure.Orm.QuestionModule
{
    public class MapperQuestion : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.Property(q => q.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(q => q.Statement)
                .IsRequired()
                .HasMaxLength(1000);

            builder.HasOne(q => q.Subject)
                .WithMany(s => s.Questions)
                .HasForeignKey(q => q.SubjectId)
                .IsRequired();

            builder.HasMany(q => q.Alternatives)
                .WithOne()
                .HasForeignKey("QuestionId");

            builder.Navigation(q => q.Alternatives).AutoInclude();
        }
    }
}