using Microsoft.AspNetCore.Mvc;
using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.Shared;
using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.SubjectsModule;
using GeradorDeTiagoes.WebApp.ViewModels;
using GeradorDeTiagoes.WebApp.Extensions;

namespace GeradorDeTiagoes.WebApp.Controllers;

[Route("subject")]
public class SubjectController : Controller
{
    private readonly IRepository<Subject> subjectRepository;
    private readonly IRepository<Discipline> disciplineRepository;

    public SubjectController(
        IRepository<Subject> subjectRepository,
        IRepository<Discipline> disciplineRepository)
    {
        this.subjectRepository = subjectRepository;
        this.disciplineRepository = disciplineRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var subjects = subjectRepository.GetAllRegisters();
        var viewModel = new SubjectListViewModel(subjects);
        return View(viewModel);
    }

    [HttpGet("register")]
    public IActionResult Create()
    {
        ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
        ViewBag.GradeLevels = GetGradeLevels();
        return View(new SubjectFormViewModel());
    }

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(SubjectFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
            ViewBag.GradeLevels = GetGradeLevels();
            return View(viewModel);
        }

        var existingSubject = subjectRepository.GetAllRegisters()
            .FirstOrDefault(s => s.Name.Equals(viewModel.Name, StringComparison.OrdinalIgnoreCase));

        if (existingSubject != null)
        {
            ModelState.AddModelError("Name", "Já existe uma matéria com este nome.");
            ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
            ViewBag.GradeLevels = GetGradeLevels();
            return View(viewModel);
        }

        var subject = viewModel.ToEntity();
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
        return View(subject.ToFormViewModel());
    }

    [HttpPost("edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Guid id, SubjectFormViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
            ViewBag.GradeLevels = GetGradeLevels();
            return View(viewModel);
        }

        var subject = subjectRepository.GetRegisterById(id);
        if (subject == null)
            return NotFound();

        var duplicateSubject = subjectRepository.GetAllRegisters()
            .FirstOrDefault(s => s.Name.Equals(viewModel.Name, StringComparison.OrdinalIgnoreCase) && s.Id != id);

        if (duplicateSubject != null)
        {
            ModelState.AddModelError("Name", "Já existe uma matéria com este nome.");
            ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
            ViewBag.GradeLevels = GetGradeLevels();
            return View(viewModel);
        }

        var updatedSubject = viewModel.ToEntity();
        subject.Update(updatedSubject);

        subjectRepository.Edit(id, subject);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id:guid}")]
    public IActionResult Details(Guid id)
    {
        var subject = subjectRepository.GetRegisterById(id);
        if (subject == null)
            return NotFound();

        return View(subject.ToDetailsViewModel());
    }

    [HttpGet("delete/{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var subject = subjectRepository.GetRegisterById(id);
        if (subject == null)
            return NotFound();

        if (subject.Questions.Count > 0)
        {
            var detailsVM = subject.ToDetailsViewModel();
            ViewBag.ErrorMessage = "Não é possível excluir esta matéria pois existem questões vinculadas a ela.";
            return View("Details", detailsVM);
        }

        return View(subject.ToDetailsViewModel());
    }

    [HttpPost("delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(Guid id)
    {
        var subject = subjectRepository.GetRegisterById(id);
        if (subject == null)
            return NotFound();

        if (subject.Questions.Count > 0)
        {
            var detailsVM = subject.ToDetailsViewModel();
            ViewBag.ErrorMessage = "Não é possível excluir esta matéria pois existem questões vinculadas a ela.";
            return View("Details", detailsVM);
        }

        subjectRepository.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    private List<string> GetGradeLevels()
    {
        return new List<string>
        {
            "1º Ano", "2º Ano", "3º Ano", "4º Ano", "5º Ano",
            "6º Ano", "7º Ano", "8º Ano", "9º Ano",
            "1ª Série EM", "2ª Série EM", "3ª Série EM"
        };
    }
}