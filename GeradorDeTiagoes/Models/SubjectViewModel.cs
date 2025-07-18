using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.WebApp.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeradorDeTiagoes.WebApp.ViewModels;

public class SubjectFormViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O campo 'Nome' é obrigatório.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O campo 'Série' é obrigatório.")]
    public string GradeLevel { get; set; }

    [Required(ErrorMessage = "O campo 'Disciplina' é obrigatório.")]
    public Guid DisciplineId { get; set; }

    public SelectList Disciplines { get; set; }

    public List<string> GradeLevels { get; set; }
}

public class SubjectDetailsViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string GradeLevel { get; set; }
    public Guid DisciplineId { get; set; }
    public string DisciplineName { get; set; }
    public int QuestionCount { get; set; }

    public SubjectDetailsViewModel(
        Guid id,
        string name,
        string gradeLevel,
        Guid disciplineId,
        string disciplineName,
        int questionCount)
    {
        Id = id;
        Name = name;
        GradeLevel = gradeLevel;
        DisciplineId = disciplineId;
        DisciplineName = disciplineName;
        QuestionCount = questionCount;
    }
}

public class SubjectListViewModel
{
    public List<SubjectDetailsViewModel> Subjects { get; set; }

    public SubjectListViewModel(List<Subject> subjects)
    {
        Subjects = subjects.ConvertAll(s => s.ToDetailsViewModel());
    }
}
public class DisciplineOptionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}