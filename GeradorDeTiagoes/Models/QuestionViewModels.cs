using GeradorDeTiagoes.Domain.QuestionModule;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GeradorDeTiagoes.WebApp.Models
{
    public class QuestionFormViewModel
    {
        [Required(ErrorMessage = "O enunciado da questão é obrigatório.")]
        public string Statement { get; set; }

        [Required(ErrorMessage = "A matéria é obrigatória.")]
        public Guid SubjectId { get; set; }
        public SelectList Disciplines { get; set; }
        public Guid DisciplineId { get; set; }
        public SelectList Subjects { get; set; }

        [Required(ErrorMessage = "É necessário pelo menos uma alternativa.")]
        [MinLength(2, ErrorMessage = "Informe ao menos duas alternativas.")]
        public List<AlternativeViewModel> Alternatives { get; set; }
    }

    public class AlternativeViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }

    public class RegisterQuestionViewModel : QuestionFormViewModel
    {
        public Guid DisciplineId { get; set; }
        public RegisterQuestionViewModel()
        {
            Alternatives = new List<AlternativeViewModel>();
            Alternatives.Add(new AlternativeViewModel());
            Alternatives.Add(new AlternativeViewModel());
        }
    }

    public class EditQuestionViewModel : QuestionFormViewModel
    {
        public Guid Id { get; set; }
        public Guid DisciplineId { get; set; }

        public EditQuestionViewModel()
        {
            Alternatives = new List<AlternativeViewModel>();
        }
    }

    public class DeleteQuestionViewModel
    {
        public Guid Id { get; set; }
        public string Statement { get; set; }

        public DeleteQuestionViewModel() { }

        public DeleteQuestionViewModel(Guid id, string statement)
        {
            Id = id;
            Statement = statement;
        }
    }

    public class ViewQuestionViewModel
    {
        public List<QuestionDetailsViewModel> Questions { get; set; }

        public ViewQuestionViewModel(List<Question> questions)
        {
            Questions = new List<QuestionDetailsViewModel>();

            foreach (var question in questions)
            {
                Questions.Add(new QuestionDetailsViewModel(question));
            }
        }
    }

    public class QuestionDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Statement { get; set; }
        public string SubjectName { get; set; }
        public string DisciplineName { get; set; }
        public int AlternativesCount => Alternatives?.Count ?? 0;
        public List<AlternativeViewModel> Alternatives { get; set; }

        public QuestionDetailsViewModel(Question question)
        {
            Id = question.Id;
            Statement = question.Statement;
            SubjectName = question.Subject?.Name ?? "Indefinida";
            DisciplineName = question.Subject?.Discipline?.Name ?? "Indefinida";
            Alternatives = question.Alternatives?.Select(a => new AlternativeViewModel
            {
                Text = a.Text,
                IsCorrect = a.IsCorrect
            }).ToList() ?? new List<AlternativeViewModel>();
        }
    }
}
