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
                Alternatives = viewModel.Alternatives.Select(a => new Alternative
                {
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

                Id = Guid.NewGuid(),
                Statement = viewModel.Statement,
                Subject = subject,
                SubjectId = subject.Id, 
                Alternatives = viewModel.Alternatives.Select(a => new Alternative
                {
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
                SubjectId = question.Subject.Id,
                Alternatives = question.Alternatives.Select(a => new AlternativeViewModel
                {
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };
        }

        public static DeleteQuestionViewModel ToDeleteQuestionViewModel(this Question question)
        {
            return new DeleteQuestionViewModel
            {
                Id = question.Id,
                Statement = question.Statement
            };
        }
    }
}
