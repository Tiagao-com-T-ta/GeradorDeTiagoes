using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.WebApp.Models;

namespace GeradorDeTiagoes.WebApp.Models.Extensions
{
    public static class QuestionExtensions
    {
        public static Question ToQuestion(this RegisterQuestionViewModel viewModel, Subject subject)
        {
            var question = new Question
            {
                Id = Guid.NewGuid(),
                Statement = viewModel.Statement,
                Subject = subject,
                SubjectId = subject.Id,
                Alternatives = viewModel.Alternatives.Select(a => new Alternative
                {
                    Id = Guid.NewGuid(),  
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };

            return question;
        }

        public static Question ToQuestion(this EditQuestionViewModel viewModel, Subject subject)
        {
            var question = new Question
            {
                Id = viewModel.Id,
                Statement = viewModel.Statement,
                Subject = subject,
                SubjectId = subject.Id,
                Alternatives = viewModel.Alternatives.Select(a => new Alternative
                {
                    Id = a.Id != Guid.Empty ? a.Id : Guid.NewGuid(),
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };
            return question;
        }

        public static EditQuestionViewModel ToEditQuestionViewModel(this Question question)
        {
            return new EditQuestionViewModel
            {
                Id = question.Id,
                Statement = question.Statement,
                SubjectId = question.Subject?.Id ?? Guid.Empty,  
                DisciplineId = question.Subject?.DisciplineId ?? Guid.Empty,  
                Alternatives = question.Alternatives?.Select(a => new AlternativeViewModel
                {
                    Id = a.Id,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList() ?? new List<AlternativeViewModel>()
            };
        }

        public static DeleteQuestionViewModel ToDeleteQuestionViewModel(this Question question)
        {
            return new DeleteQuestionViewModel(question.Id, question.Statement);
        }
    }
}