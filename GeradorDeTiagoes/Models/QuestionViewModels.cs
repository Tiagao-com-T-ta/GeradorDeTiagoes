using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.WebApp.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GeradorDeTiagoes.WebApp.Models
{
    public abstract class QuestionFormViewModel
    {
        [Required(ErrorMessage = "O enunciado da questão é obrigatório.")]
        public string Text { get; set; }
        
        [Required(ErrorMessage = "O nome da disciplina é obrigatório.")]
        public string SubjectName { get; set; }

        public List<SelectListItem> Subjects { get; set; }
        public List<Alternative> Alternatives { get; set; }
    }

    public class RegisterQuestionViewModel : QuestionFormViewModel
    {
        public RegisterQuestionViewModel() { }

        public RegisterQuestionViewModel(string text, string subjectName, List<Alternative> alternatives)
        {
            Text = text;
            SubjectName = subjectName;
            Alternatives = alternatives;
        }
    }

    public class EditQuestionViewModel : QuestionFormViewModel
    {
        public Guid Id { get; set; }

        public EditQuestionViewModel() { }
        public EditQuestionViewModel(Guid id, string text, List<Alternative> alternatives)
        {
            Id = id;
            Text = text;
            Alternatives = alternatives;
        }
    }

    public class DeleteQuestionViewModel : QuestionFormViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public DeleteQuestionViewModel() { }
        public DeleteQuestionViewModel(Guid id, string text)
        {
            Id = id;
            Text = text;
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
                Questions.Add(question.ToDetailsVM());
            }
        }
    }

    public class QuestionDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string SubjectName { get; set; }
        public List<Alternative> Alternatives { get; set; }
        public QuestionDetailsViewModel(Guid id, string text, string subjectName, List<Alternative> alternatives)
        {
            Id = id;
            Text = text;
            SubjectName = subjectName;
            Alternatives = alternatives;
        }
    }

    public class  SelectQuestionViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public SelectQuestionViewModel(Guid id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
