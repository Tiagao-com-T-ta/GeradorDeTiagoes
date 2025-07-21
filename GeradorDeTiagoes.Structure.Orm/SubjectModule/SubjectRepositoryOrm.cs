using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.SubjectsModule;
using GeradorDeTiagoes.Structure.Orm.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Structure.Orm.DisciplineModule
{
    public class SubjectRepositoryOrm : BaseRepositoryOrm<Subject>, ISubjectRepository
    {
        public SubjectRepositoryOrm(GeradorDeTiagoesDbContext context) : base(context) { }

    }
}
