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
using GeradorDeTiagoes.Structure.Orm.Shared;

namespace GeradorDeTiagoes.WebApp.Controllers
{
    [Route("question")]
    public class QuestionController : Controller
    {
        private readonly GeradorDeTiagoesDbContext dataContext;
        private readonly IRepository<Question> questionRepository;
        private readonly IRepository<Subject> subjectRepository;
        private readonly IRepository<Discipline> disciplineRepository;

        public QuestionController(
            IRepository<Question> questionRepository,
            IRepository<Subject> subjectRepository,
            IRepository<Discipline> disciplineRepository,
            GeradorDeTiagoesDbContext dataContext)
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

            dataContext.SaveChanges();

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

            viewModel.SubjectId = question.Subject.Id;

            LoadDisciplinesSubjects(viewModel);

            return View(viewModel);
        }

        [HttpPost("edit/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, EditQuestionViewModel viewModel)
        {
            var existingQuestion = questionRepository.GetRegisterById(id);
            if (existingQuestion == null)
                return NotFound();

            var subject = subjectRepository.GetRegisterById(viewModel.SubjectId);
            if (subject == null)
                return NotFound(); 

            existingQuestion.Statement = viewModel.Statement;
            existingQuestion.Subject = subject;
            existingQuestion.SubjectId = subject.Id;

            dataContext.Alternatives.RemoveRange(existingQuestion.Alternatives);

            existingQuestion.Alternatives = viewModel.Alternatives
                .Where(a => !string.IsNullOrWhiteSpace(a.Text))
                .Select(a => new Alternative
                {
                    Id = a.Id != Guid.Empty ? a.Id : Guid.NewGuid(),
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                })
                .ToList();

            var transaction = dataContext.Database.BeginTransaction();

            try
            {
                questionRepository.Edit(id, existingQuestion);
                dataContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
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

        [HttpPost("deleteconfirmed/{id:guid}")]
        [ValidateAntiForgeryToken]
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
            
            var transaction = dataContext.Database.BeginTransaction();

            try
            {
                questionRepository.Delete(id);

                dataContext.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadDisciplinesSubjects(QuestionFormViewModel viewModel)
        {

            var disciplines = disciplineRepository.GetAllRegisters();
            viewModel.Disciplines = new SelectList(disciplines, "Id", "Name");


            if (viewModel.DisciplineId != Guid.Empty)
            {
                var subjects = subjectRepository.GetAllRegisters()
                    .Where(s => s.DisciplineId == viewModel.DisciplineId)
                    .ToList();

                viewModel.Subjects = new SelectList(subjects, "Id", "Name");
            }
            else if (viewModel.SubjectId != Guid.Empty)
            {

                var subject = subjectRepository.GetRegisterById(viewModel.SubjectId);
                if (subject != null)
                {
                    viewModel.DisciplineId = subject.DisciplineId;
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
