using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GeradorDeTiagoes.WebApp.Extensions
{
    public static class QuestionExtensions
    {
        public static Question ToEntity(this QuestionFormViewModel registerVM)
        {
            var question = new Question
            {
                Text = registerVM.Text,
                SubjectName = registerVM.SubjectName,
                Alternatives = registerVM.Alternatives
            };

            return question;
        }

        public static QuestionDetailsViewModel ToDetailsVM(this Question question)
        {
            return new QuestionDetailsViewModel
            (
                question.Id,
                question.Text,
                question.SubjectName,
                question.Alternatives
            );
        }
    }
}
