using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.Domain.SubjectsModule;
using GeradorDeTiagoes.Structure.Files.QuestionModule;
using GeradorDeTiagoes.Structure.Files.Shared;
using GeradorDeTiagoes.Structure.Files.SubjectsModule;
using GeradorDeTiagoes.WebApp.Extensions;
using GeradorDeTiagoes.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GeradorDeTiagoes.WebApp.Controllers
{
    [Route("questions")]
    public class QuestionController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IQuestionRepository questionRepository;
        private readonly ISubjectRepository subjectRepository;

        public QuestionController()
        {
            dataContext = new DataContext(true);
            questionRepository = new QuestionRepositoryFile(dataContext);
            subjectRepository = new SubjectRepositoryFile(dataContext);
        }

        public IActionResult Index()
        {
            var questions = questionRepository.GetAllRegisters();

            var viewVM = new ViewQuestionViewModel(questions);

            return View();
        }

        [HttpGet("register")]
        public IActionResult Create()
        {
            var registerVM = new RegisterQuestionViewModel();
            registerVM.Subjects = subjectRepository.GetAllRegisters()
            .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
            .ToList();

            return View(registerVM);
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RegisterQuestionViewModel registerVM)
        {
            var registers = questionRepository.GetAllRegisters();

            var entity = registerVM.ToEntity();

            questionRepository.Register(entity);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            var selectedQuestion = questionRepository.GetRegisterById(id);

            var editVM = new EditQuestionViewModel(
                selectedQuestion.Id,
                selectedQuestion.Text,
                selectedQuestion.Alternatives
                );

            return View(editVM);
        }

        [HttpPost("edit/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, EditQuestionViewModel editVM)
        {
            var registers = questionRepository.GetAllRegisters();

            var entity = editVM.ToEntity();

            questionRepository.Edit(id, entity);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("delete/{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var selectedRegister = questionRepository.GetRegisterById(id);

            var deleteVM = new DeleteQuestionViewModel(
                selectedRegister.Id,
                selectedRegister.Text
            );

            return View(deleteVM);

        }

        [HttpPost("delete/{id:guid}")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var isInTest = dataContext.Tests.Any(t => t.Questions.Any(q => q.Id == id));
            if (isInTest)
            {
                ModelState.AddModelError("Em Uso", "Não é possível excluir esta questão pois ela está vinculada a um teste.");
                var question = questionRepository.GetRegisterById(id);
                var deleteVM = new DeleteQuestionViewModel(
                    question.Id,
                    question.Text
                );
                return View(deleteVM);
            }

            questionRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("details/{id:guid}")]
        public IActionResult Details(Guid id)
        {
            var selectedQuestion = questionRepository.GetRegisterById(id);

            var detailsVM = new QuestionDetailsViewModel(
                selectedQuestion.Id,
                selectedQuestion.Text,
                selectedQuestion.SubjectName,
                selectedQuestion.Alternatives
            );

            return View(detailsVM);
        }
    }
}
