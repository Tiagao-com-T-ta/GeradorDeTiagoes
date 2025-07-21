using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Structure.Orm.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Structure.Orm.DisciplineModule
{
    public class DisciplineRepositoryOrm : BaseRepositoryOrm<Discipline>, IDisciplineRepository
    {
        public DisciplineRepositoryOrm(GeradorDeTiagoesDbContext context) : base(context) { }

    }
}
