using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.WebApp.ViewModels;

namespace GeradorDeTiagoes.WebApp.Extensions
{
    public static class TestExtensions
    {
        public static Test ToEntity(this TestFormViewModel viewModel)
        {
            return new Test(
                viewModel.Title,
                viewModel.GradeLevel,
                viewModel.DisciplineId,
                viewModel.IsRecovery ? null : viewModel.SubjectId,
                viewModel.QuestionCount,
                viewModel.IsRecovery
            );
        }

        public static TestDetailsViewModel ToDetailsViewModel(this Test test)
        {
            return new TestDetailsViewModel(
                test.Id,
                test.Title,
                test.GradeLevel,
                test.DisciplineId,
                test.Discipline?.Name,
                test.SubjectId,
                test.Subject?.Name,
                test.QuestionCount,
                test.IsRecovery,
                test.Questions?.Count ?? 0
            );
        }

        public static TestFormViewModel ToFormViewModel(this Test test)
        {
            return new TestFormViewModel
            {
                Id = test.Id,
                Title = test.Title,
                GradeLevel = test.GradeLevel,
                DisciplineId = test.DisciplineId,
                SubjectId = test.IsRecovery ? null : test.SubjectId,
                QuestionCount = test.QuestionCount,
                IsRecovery = test.IsRecovery
            };
        }

        public static TestDuplicateViewModel ToDuplicateViewModel(this Test test)
        {
            return new TestDuplicateViewModel
            {
                OriginalTestId = test.Id,
                Title = $"{test.Title} (Cópia)",
                GradeLevel = test.GradeLevel,
                DisciplineId = test.DisciplineId,
                SubjectId = test.IsRecovery ? null : test.SubjectId,
                QuestionCount = test.QuestionCount,
                IsRecovery = false
            };
        }

        public static Test ToEntityWithClonedQuestions(this TestDuplicateViewModel viewModel, Test originalTest)
        {
            var newTest = new Test(
                viewModel.Title,
                viewModel.GradeLevel,
                viewModel.DisciplineId,
                viewModel.IsRecovery ? null : viewModel.SubjectId,
                viewModel.QuestionCount,
                viewModel.IsRecovery
            );

            newTest.Questions = originalTest.Questions.Select(originalQ => new Question
            {
                Id = Guid.NewGuid(),
                Statement = originalQ.Statement,
                SubjectId = originalQ.SubjectId,
                Subject = originalQ.Subject,
                Alternatives = viewModel.IsRecovery
                    ? new List<Alternative>()
                    : originalQ.Alternatives.Select(a => new Alternative
                    {
                        Id = Guid.NewGuid(),
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
            }).ToList();

            return newTest;
        }
    }
}