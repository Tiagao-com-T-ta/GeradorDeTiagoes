using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.WebApp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeradorDeTiagoes.WebApp.ViewModels
{
    public class TestFormViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo 'Título' é obrigatório")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O título deve ter entre 2 e 100 caracteres")]
        public string Title { get; set; }

        [Required(ErrorMessage = "O campo 'Série' é obrigatório")]
        public string GradeLevel { get; set; }

        [Required(ErrorMessage = "O campo 'Disciplina' é obrigatório")]
        public Guid DisciplineId { get; set; }

        public Guid? SubjectId { get; set; }

        [Required(ErrorMessage = "O campo 'Quantidade de Questões' é obrigatório")]
        [Range(1, 100, ErrorMessage = "A quantidade deve ser entre 1 e 100")]
        public int QuestionCount { get; set; }

        public bool IsRecovery { get; set; }

        public static List<string> GradeLevels => new()
        {
            "1º Ano", "2º Ano", "3º Ano", "4º Ano", "5º Ano",
            "6º Ano", "7º Ano", "8º Ano", "9º Ano",
            "1ª Série EM", "2ª Série EM", "3ª Série EM"
        };
    }

    public class TestDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string GradeLevel { get; set; }
        public Guid DisciplineId { get; set; }
        public string DisciplineName { get; set; }
        public Guid? SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int QuestionCount { get; set; }
        public bool IsRecovery { get; set; }
        public int QuestionsInTest { get; set; }

        public TestDetailsViewModel(
            Guid id,
            string title,
            string gradeLevel,
            Guid disciplineId,
            string disciplineName,
            Guid? subjectId,
            string subjectName,
            int questionCount,
            bool isRecovery,
            int questionsInTest)
        {
            Id = id;
            Title = title;
            GradeLevel = gradeLevel;
            DisciplineId = disciplineId;
            DisciplineName = disciplineName;
            SubjectId = subjectId;
            SubjectName = subjectName;
            QuestionCount = questionCount;
            IsRecovery = isRecovery;
            QuestionsInTest = questionsInTest;
        }
    }

    public class TestListViewModel
    {
        public List<TestDetailsViewModel> Tests { get; set; }

        public TestListViewModel(
            List<Test> tests,
            List<Discipline> disciplines,
            List<Subject> subjects)
        {
            Tests = tests.ConvertAll(t =>
            {
                var discipline = disciplines.FirstOrDefault(d => d.Id == t.DisciplineId);
                var subject = t.SubjectId.HasValue
                    ? subjects.FirstOrDefault(s => s.Id == t.SubjectId.Value)
                    : null;

                return new TestDetailsViewModel(
                    t.Id,
                    t.Title,
                    t.GradeLevel,
                    t.DisciplineId,
                    discipline?.Name ?? "",
                    t.SubjectId,
                    subject?.Name ?? "",
                    t.QuestionCount,
                    t.IsRecovery,
                    t.Questions?.Count ?? 0
                );
            });
        }
    }




    public class TestDuplicateViewModel : TestFormViewModel
    {
        public Guid OriginalTestId { get; set; }
    }

    public class PdfOptionsViewModel
    {
        public Guid TestId { get; set; }
        public bool IncludeAnswers { get; set; }
    }

}