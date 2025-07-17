using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.WebApp.ViewModels;

namespace GeradorDeTiagoes.WebApp.Extensions;

public static class SubjectExtensions
{
    public static Subject ToEntity(this SubjectFormViewModel viewModel)
    {
        return new Subject(
            viewModel.Name,
            viewModel.GradeLevel,
            viewModel.DisciplineId
        );
    }

    public static SubjectDetailsViewModel ToDetailsViewModel(this Subject subject)
    {
        return new SubjectDetailsViewModel(
            subject.Id,
            subject.Name,
            subject.GradeLevel,
            subject.DisciplineId,
            subject.Discipline?.Name,
            subject.Questions.Count
        );
    }

    public static SubjectFormViewModel ToFormViewModel(this Subject subject)
    {
        return new SubjectFormViewModel
        {
            Id = subject.Id,
            Name = subject.Name,
            GradeLevel = subject.GradeLevel,
            DisciplineId = subject.DisciplineId
        };
    }
}