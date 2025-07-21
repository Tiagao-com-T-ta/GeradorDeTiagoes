using GeradorDeTiagoes.Domain.DisciplineModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Structure.Orm.DisciplineModule
{
    public  class MapperDiscipline : IEntityTypeConfiguration<Discipline>
    {
        public void Configure(EntityTypeBuilder<Discipline> builder) 
        {
            builder.Property(d => d.Id)
                .IsRequired()
                .ValueGeneratedNever();

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}
