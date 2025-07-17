using GeradorDeTiagoes.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Domain.DisciplineModule
{
    public class Discipline : BaseEntity<Discipline>
    {
        public string Name { get; set; }

        public Discipline() { }

        public Discipline(string name)
        {
            Name = name;
        }   

        public override void Update(Discipline EditedEntity)
        {
            Name = EditedEntity.Name;
        }
    }
}
