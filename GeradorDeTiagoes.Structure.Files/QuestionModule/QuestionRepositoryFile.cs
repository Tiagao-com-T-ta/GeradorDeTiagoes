using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.Structure.Files.Shared;

namespace GeradorDeTiagoes.Structure.Files.QuestionModule
{
    public class QuestionRepositoryFile : BaseRepositoryFile<Question>, IQuestionRepository
    {
        public QuestionRepositoryFile(DataContext dataContext) : base(dataContext)
        {
        }

        protected override List<Question> GetRegisters()
        {
            return dataContext.Questions;
        }

        public List<Question> GetAllBySubject(Guid subjectId)
        {
            return registers.Where(q => q.Subject.Id == subjectId).ToList();
        }

        public List<Question> GetAllByDiscipline(Guid disciplineId)
        {
            return registers.Where(q => q.Subject.Discipline.Id == disciplineId).ToList();
        }
    }
}
