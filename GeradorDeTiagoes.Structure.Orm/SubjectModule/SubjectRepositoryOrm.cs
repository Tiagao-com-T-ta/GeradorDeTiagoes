using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.SubjectsModule;
using GeradorDeTiagoes.Structure.Orm.Shared;
using Microsoft.EntityFrameworkCore;

namespace GeradorDeTiagoes.Structure.Orm.DisciplineModule
{
    public class SubjectRepositoryOrm : BaseRepositoryOrm<Subject>, ISubjectRepository
    {
        public SubjectRepositoryOrm(GeradorDeTiagoesDbContext context) : base(context)
        {
        }

        public override Subject? GetRegisterById(Guid id)
        {
            return dbContext.Subjects
                .Include(s => s.Discipline)
                .Include(s => s.Questions)
                .FirstOrDefault(s => s.Id == id);
        }
    }
}
