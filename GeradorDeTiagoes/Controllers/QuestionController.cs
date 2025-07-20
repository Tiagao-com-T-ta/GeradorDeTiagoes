using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.Domain.SubjectsModule;
using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.WebApp.Models;
using GeradorDeTiagoes.WebApp.Models.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using GeradorDeTiagoes.Domain.Shared;
using GeradorDeTiagoes.Structure.Files.Shared;

namespace GeradorDeTiagoes.WebApp.Controllers
{
    [Route("question")]
    public class QuestionController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IRepository<Question> questionRepository;
        private readonly IRepository<Subject> subjectRepository;
        private readonly IRepository<Discipline> disciplineRepository;

        public QuestionController(
            IRepository<Question> questionRepository,
            IRepository<Subject> subjectRepository,
            IRepository<Discipline> disciplineRepository,
            DataContext dataContext)
        {
            this.dataContext = dataContext;
            this.questionRepository = questionRepository;
            this.subjectRepository = subjectRepository;
            this.disciplineRepository = disciplineRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var questions = questionRepository.GetAllRegisters();

            foreach (var question in questions)
            {
                question.Subject = subjectRepository.GetRegisterById(question.Subject.Id);
                question.Subject.Discipline = disciplineRepository.GetRegisterById(question.Subject.DisciplineId);
            }

            var viewModel = new ViewQuestionViewModel(questions);
            return View(viewModel);
        }

        [HttpGet("register")]
        public IActionResult Create(Guid? disciplineId = null)
        {
            var viewModel = new RegisterQuestionViewModel();

            if (disciplineId.HasValue)
                viewModel.DisciplineId = disciplineId.Value;

            LoadDisciplinesSubjects(viewModel);

            return View(viewModel);
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RegisterQuestionViewModel viewModel)
        {
            LoadDisciplinesSubjects(viewModel);

            if (!ValidateAlternatives(viewModel.Alternatives))
            {
                ModelState.AddModelError("", "A questão deve ter entre 2 e 4 alternativas, com exatamente 1 alternativa correta.");
            }

            var subject = subjectRepository.GetRegisterById(viewModel.SubjectId);
            if (subject == null)
            {
                ModelState.AddModelError("SubjectId", "Matéria inválida.");
                return View(viewModel);
            }

            var question = viewModel.ToQuestion(subject);
            questionRepository.Register(question);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id:guid}")]
        public IActionResult Edit(Guid id)
        {
            var question = questionRepository.GetRegisterById(id);
            if (question == null)
                return NotFound();

            question.Subject = subjectRepository.GetRegisterById(question.Subject.Id);
            question.Subject.Discipline = disciplineRepository.GetRegisterById(question.Subject.DisciplineId);

            var viewModel = question.ToEditQuestionViewModel();

            LoadDisciplinesSubjects(viewModel);

            return View(viewModel);
        }

        [HttpPost("edit/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, EditQuestionViewModel viewModel)
        {
            LoadDisciplinesSubjects(viewModel);

            if (!ValidateAlternatives(viewModel.Alternatives))
            {
                ModelState.AddModelError("", "A questão deve ter entre 2 e 4 alternativas, com exatamente 1 alternativa correta.");
            }

            if (!ModelState.IsValid)
                return View(viewModel);

            var subject = subjectRepository.GetRegisterById(viewModel.SubjectId);
            if (subject == null)
            {
                ModelState.AddModelError("SubjectId", "Matéria inválida.");
                return View(viewModel);
            }

            var updatedQuestion = viewModel.ToQuestion(subject);

            questionRepository.Edit(id, updatedQuestion);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("details/{id:guid}")]
        public IActionResult Details(Guid id)
        {
            var question = questionRepository.GetRegisterById(id);
            if (question == null)
                return NotFound();

            question.Subject = subjectRepository.GetRegisterById(question.Subject.Id);
            question.Subject.Discipline = disciplineRepository.GetRegisterById(question.Subject.DisciplineId);

            var viewModel = new QuestionDetailsViewModel(question);

            return View(viewModel);
        }

        [HttpGet("delete/{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var question = questionRepository.GetRegisterById(id);
            if (question == null)
                return NotFound();

            var viewModel = question.ToDeleteQuestionViewModel();

            return View(viewModel);
        }

        [HttpPost("delete/{id:guid}")]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var question = questionRepository.GetRegisterById(id);
            if (question == null)
                return NotFound();

            var isUsedInTests = dataContext.Tests.Any(t => t.Questions.Any(q => q.Id == id));
            if (isUsedInTests)
            {
                TempData["ErrorMessage"] = "Não é possível excluir a questão pois está vinculada a um teste.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            questionRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private void LoadDisciplinesSubjects(QuestionFormViewModel viewModel)
        {
            var disciplines = disciplineRepository.GetAllRegisters();
            viewModel.Disciplines = new SelectList(disciplines, "Id", "Name");

            if (viewModel is RegisterQuestionViewModel registerVM && registerVM.DisciplineId != Guid.Empty)
            {
                var subjects = subjectRepository.GetAllRegisters()
                    .Where(s => s.DisciplineId == registerVM.DisciplineId)
                    .ToList();

                viewModel.Subjects = new SelectList(subjects, "Id", "Name");
            }
            else if (viewModel.SubjectId != Guid.Empty)
            {
                var subject = subjectRepository.GetRegisterById(viewModel.SubjectId);
                if (subject != null)
                {
                    var subjects = subjectRepository.GetAllRegisters()
                        .Where(s => s.DisciplineId == subject.DisciplineId)
                        .ToList();

                    viewModel.Subjects = new SelectList(subjects, "Id", "Name");
                }
            }
            else
            {
                viewModel.Subjects = new SelectList(new List<Subject>(), "Id", "Name");
            }
        }

        private bool ValidateAlternatives(System.Collections.Generic.List<AlternativeViewModel> alternatives)
        {
            if (alternatives == null)
                return false;

            var correctCount = alternatives.Count(a => a.IsCorrect);
            var totalCount = alternatives.Count;

            return totalCount >= 2 && totalCount <= 4 && correctCount == 1;
        }

        [HttpGet("GetSubjectsByDiscipline")]
        public IActionResult GetSubjectsByDiscipline(Guid disciplineId)
        {
            var subjects = subjectRepository.GetAllRegisters()
                .Where(s => s.DisciplineId == disciplineId)
                .Select(s => new { id = s.Id, name = s.Name })
                .ToList();

            return Json(subjects);
        }

    }

}
