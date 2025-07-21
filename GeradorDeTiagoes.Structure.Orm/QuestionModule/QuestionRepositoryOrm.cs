using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.Structure.Orm.Shared;
using Microsoft.EntityFrameworkCore;

namespace GeradorDeTiagoes.Structure.Orm.QuestionModule
{
    public class QuestionRepositoryOrm : BaseRepositoryOrm<Question>, IQuestionRepository
    {
        public QuestionRepositoryOrm(GeradorDeTiagoesDbContext context) : base(context)
        {
        }

        public override Question? GetRegisterById(Guid id)
        {
            return dbContext.Questions
                .Include(q => q.Subject)
                    .ThenInclude(s => s.Discipline)
                .Include(q => q.Alternatives)
                .FirstOrDefault(q => q.Id == id);
        }

        public List<Question> GetAllBySubject(Guid subjectId)
        {
            return dbContext.Questions
                .Include(q => q.Subject)
                .Include(q => q.Alternatives)
                .Where(q => q.SubjectId == subjectId)
                .ToList();
        }

        public List<Question> GetAllByDiscipline(Guid disciplineId)
        {
            return dbContext.Questions
                .Include(q => q.Subject)
                    .ThenInclude(s => s.Discipline)
                .Include(q => q.Alternatives)
                .Where(q => q.Subject.Discipline.Id == disciplineId)
                .ToList();
        }

        public override List<Question> GetAllRegisters()
        {
            return dbContext.Questions
                .Include(q => q.Subject)
                    .ThenInclude(s => s.Discipline)
                .Include(q => q.Alternatives)
                .ToList();
        }

    }
}
