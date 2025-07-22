using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.Domain.Shared;
using GeradorDeTiagoes.Domain.SubjectsModule;
using GeradorDeTiagoes.Domain.TestsModule;
using GeradorDeTiagoes.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GeradorDeTiagoes.WebApp.Validators
{
    public class TestValidator
    {
        private readonly IRepository<Test> testRepository;
        private readonly IRepository<Subject> subjectRepository;
        private readonly IQuestionRepository questionRepository;

        public TestValidator(
            IRepository<Test> testRepository,
            IRepository<Subject> subjectRepository,
            IQuestionRepository questionRepository)
        {
            this.testRepository = testRepository;
            this.subjectRepository = subjectRepository;
            this.questionRepository = questionRepository;
        }

        public bool TitleExists(string title)
        {
            return testRepository.GetAllRegisters()
                .Any(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public bool ValidateSubject(TestFormViewModel viewModel, ModelStateDictionary modelState)
        {
            if (!viewModel.SubjectId.HasValue)
            {
                modelState.AddModelError("SubjectId", "Matéria é obrigatória.");
                return false;
            }

            var subject = subjectRepository.GetRegisterById(viewModel.SubjectId.Value);
            if (subject == null || subject.DisciplineId != viewModel.DisciplineId)
            {
                modelState.AddModelError("SubjectId", "Matéria não pertence à disciplina selecionada.");
                return false;
            }

            return true;
        }

        public List<Question> GetAvailableQuestions(TestFormViewModel viewModel, ModelStateDictionary modelState)
        {
            if (viewModel.IsRecovery)
                return questionRepository.GetAllByDiscipline(viewModel.DisciplineId);

            if (viewModel.SubjectId.HasValue)
                return questionRepository.GetAllBySubject(viewModel.SubjectId.Value);

            modelState.AddModelError("SubjectId", "Matéria é obrigatória.");
            return null;
        }
    }
}
