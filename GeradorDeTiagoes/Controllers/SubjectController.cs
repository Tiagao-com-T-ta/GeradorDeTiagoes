using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.Shared;
using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.SubjectsModule;
using GeradorDeTiagoes.WebApp.ViewModels;
using GeradorDeTiagoes.WebApp.Extensions;
using GeradorDeTiagoes.Structure.Files.Shared;

namespace GeradorDeTiagoes.WebApp.Controllers;

[Route("subject")]
public class SubjectController : Controller
{
    private readonly DataContext dataContext;
    private readonly IRepository<Subject> subjectRepository;
    private readonly IRepository<Discipline> disciplineRepository;

    public SubjectController(
        IRepository<Subject> subjectRepository,
        IRepository<Discipline> disciplineRepository,
        DataContext dataContext)
    {
        this.dataContext = dataContext;
        this.subjectRepository = subjectRepository;
        this.disciplineRepository = disciplineRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var subjects = subjectRepository.GetAllRegisters();

        foreach (var subject in subjects)
        {
            subject.Discipline = disciplineRepository.GetRegisterById(subject.DisciplineId);
        }

        var viewModel = new SubjectListViewModel(subjects);
        return View(viewModel);
    }

    [HttpGet("register")]
    public IActionResult Create()
    {
        var disciplinas = disciplineRepository.GetAllRegisters();

        var viewModel = new SubjectFormViewModel
        {
            Disciplines = new SelectList(disciplinas, "Id", "Name"),
            GradeLevels = GetGradeLevels()
        };

        return View(viewModel);
    }

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(SubjectFormViewModel viewModel)
    {
        var subject = new Subject
        {
            Id = Guid.NewGuid(),
            Name = viewModel.Name,
            GradeLevel = viewModel.GradeLevel,
            DisciplineId = viewModel.DisciplineId,
            Discipline = disciplineRepository.GetRegisterById(viewModel.DisciplineId)
        };

        subjectRepository.Register(subject);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:guid}")]
    public IActionResult Edit(Guid id)
    {
        var subject = subjectRepository.GetRegisterById(id);
        if (subject == null)
            return NotFound();

        ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
        ViewBag.GradeLevels = GetGradeLevels();

        var viewModel = new SubjectFormViewModel
        {
            Id = subject.Id,
            Name = subject.Name,
            GradeLevel = subject.GradeLevel,
            DisciplineId = subject.DisciplineId
        };

        return View(viewModel);
    }

    [HttpPost("edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, SubjectFormViewModel viewModel)
    {
        var subject = subjectRepository.GetRegisterById(id);

        if (subject == null)
            return NotFound();

        var updatedSubject = new Subject
        {
            Id = id,
            Name = viewModel.Name,
            GradeLevel = viewModel.GradeLevel,
            DisciplineId = viewModel.DisciplineId,
            Discipline = disciplineRepository.GetRegisterById(viewModel.DisciplineId)
        };

        subjectRepository.Edit(id, updatedSubject);
        return RedirectToAction(nameof(Index));
    }

    private SubjectFormViewModel FillFormViewModel(SubjectFormViewModel viewModel)
    {
        viewModel.GradeLevels = GetGradeLevels();
        viewModel.Disciplines = new SelectList(disciplineRepository.GetAllRegisters(), "Id", "Name");
        return viewModel;
    }

    private List<string> GetGradeLevels()
    {
        return new()
        {
            "1º Ano", "2º Ano", "3º Ano", "4º Ano", "5º Ano",
            "6º Ano", "7º Ano", "8º Ano", "9º Ano",
            "1ª Série EM", "2ª Série EM", "3ª Série EM"
        };
    }

    [HttpGet("details/{id:guid}")]
    public IActionResult Details(Guid id)
    {
        var subject = subjectRepository.GetRegisterById(id);
        if (subject == null)
        {
            return NotFound();
        }

        var viewModel = new SubjectDetailsViewModel(
            subject.Id,
            subject.Name,
            subject.GradeLevel,
            subject.DisciplineId,
            subject.Discipline?.Name ?? "Não informado",
            subject.Questions?.Count ?? 0
        );

        return View(viewModel);
    }
    [HttpGet("delete/{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var subject = subjectRepository.GetRegisterById(id);
        if (subject == null)
        {
            return NotFound();
        }

        var viewModel = new SubjectDetailsViewModel(
            subject.Id,
            subject.Name,
            subject.GradeLevel,
            subject.DisciplineId,
            subject.Discipline?.Name ?? "Não informado",
            subject.Questions?.Count ?? 0
        );

        return View(viewModel);
    }

    [HttpPost("delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(Guid id)
    {
        var subject = subjectRepository.GetRegisterById(id);
        if (subject == null)
        {
            return NotFound();
        }

        if (subject.Questions != null && subject.Questions.Any())
        {
            TempData["ErrorMessage"] = "Não é possível excluir a matéria pois existem questões vinculadas a ela.";
            return RedirectToAction(nameof(Delete), new { id });
        }

        subjectRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

}
