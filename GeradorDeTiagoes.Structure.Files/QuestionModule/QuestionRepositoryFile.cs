using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.Structure.Files.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Structure.Files.QuestionModule
{
    public class QuestionRepositoryFile : BaseRepositoryFile<Question>, IQuestionRepository
    {
        public QuestionRepositoryFile(DataContext dataContext) : base(dataContext) { }

        protected override List<Question> GetRegisters()
        {
            return dataContext.Questions;
        }

        public List<Question> GetByDiscipline(Guid disciplineId)
        {
            return dataContext.Questions
                .Where(q => q.Subject != null && q.Subject.DisciplineId == disciplineId)
                .ToList();
        }

        public List<Question> GetBySubject(Guid subjectId)
        {
            return dataContext.Questions
                .Where(q => q.Subject != null && q.Subject.Id == subjectId)
                .ToList();
        }
    }
}