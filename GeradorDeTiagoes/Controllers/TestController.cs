using Microsoft.AspNetCore.Mvc;
using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.Shared;
using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.SubjectsModule;
using GeradorDeTiagoes.Domain.TestsModule;
using GeradorDeTiagoes.Domain.QuestionModule;
using GeradorDeTiagoes.WebApp.ViewModels;
using GeradorDeTiagoes.WebApp.Extensions;
using GeradorDeTiagoes.Domain.PdfModule;
using GeradorDeTiagoes.Structure.Files.Shared;
using GeradorDeTiagoes.Structure.Orm.Shared;
using GeradorDeTiagoes.WebApp.Validators;

namespace GeradorDeTiagoes.WebApp.Controllers
{
    [Route("tests")]
    public class TestController : Controller
    {
        private readonly GeradorDeTiagoesDbContext dataContext;
        private readonly IRepository<Test> testRepository;
        private readonly IRepository<Discipline> disciplineRepository;
        private readonly IRepository<Subject> subjectRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IPdfGenerator pdfGenerator;
        private readonly TestValidator testValidator;

        public TestController(
            IRepository<Test> testRepository,
            IRepository<Discipline> disciplineRepository,
            IRepository<Subject> subjectRepository,
            IQuestionRepository questionRepository,
            IPdfGenerator pdfGenerator,
            GeradorDeTiagoesDbContext dataContext,
            TestValidator testValidator)
        {
            this.dataContext = dataContext;
            this.testRepository = testRepository;
            this.disciplineRepository = disciplineRepository;
            this.subjectRepository = subjectRepository;
            this.questionRepository = questionRepository;
            this.pdfGenerator = pdfGenerator;
            this.testValidator = testValidator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var tests = testRepository.GetAllRegisters().ToList();
            var disciplines = disciplineRepository.GetAllRegisters().ToList();
            var subjects = subjectRepository.GetAllRegisters().ToList();

            var viewModel = new TestListViewModel(tests, disciplines, subjects);
            return View(viewModel);
        }

        [HttpGet("gerar")]
        public IActionResult Create()
        {
            ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
            ViewBag.GradeLevels = TestFormViewModel.GradeLevels;
            return View(new TestFormViewModel());
        }

        [HttpPost("gerar")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TestFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return ReloadViewWithErrors(viewModel);

            if (testValidator.TitleExists(viewModel.Title))
            {
                ModelState.AddModelError("Title", "Já existe um teste com este título.");
                return ReloadViewWithErrors(viewModel);
            }

            if (!viewModel.IsRecovery && !testValidator.ValidateSubject(viewModel, ModelState))
                return ReloadViewWithErrors(viewModel);

            var availableQuestions = testValidator.GetAvailableQuestions(viewModel, ModelState);
            if (availableQuestions == null)
                return ReloadViewWithErrors(viewModel);

            if (availableQuestions.Count < viewModel.QuestionCount)
            {
                ModelState.AddModelError("QuestionCount",
                    $"Quantidade insuficiente de questões disponíveis ({availableQuestions.Count}).");
                return ReloadViewWithErrors(viewModel);
            }

            var test = viewModel.ToEntity();
            var random = new Random();
            test.Questions = availableQuestions
                .OrderBy(q => random.Next())
                .Take(viewModel.QuestionCount)
                .ToList();

            testRepository.Register(test);
            dataContext.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = test.Id });
        }

        [HttpGet("duplicar/{id:guid}")]
        public IActionResult Duplicate(Guid id)
        {
            var originalTest = testRepository.GetRegisterById(id);
            if (originalTest == null)
                return NotFound();

            var viewModel = originalTest.ToDuplicateViewModel();
            viewModel.IsRecovery = false;

            ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
            ViewBag.GradeLevels = TestFormViewModel.GradeLevels;

            if (viewModel.DisciplineId != Guid.Empty)
            {
                var subjects = subjectRepository.GetAllRegisters()
                    .Where(s => s.DisciplineId == viewModel.DisciplineId)
                    .ToList();

                ViewBag.Subjects = subjects;
            
            }

            return View(viewModel);
        }

        [HttpPost("duplicar/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult Duplicate(Guid id, TestDuplicateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return ReloadDuplicateView(viewModel);

            if (testValidator.TitleExists(viewModel.Title))
            {
                ModelState.AddModelError("Title", "Já existe um teste com este título.");
                return ReloadDuplicateView(viewModel);
            }

            var originalTest = testRepository.GetRegisterById(id);
            if (originalTest == null)
                return NotFound();

            var newTest = viewModel.ToEntityWithClonedQuestions(originalTest);

            testRepository.Register(newTest);
            dataContext.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = newTest.Id });
        }

        [HttpGet("detalhes/{id:guid}")]
        public IActionResult Details(Guid id)
        {
            var test = testRepository.GetRegisterById(id);
            if (test == null)
                return NotFound();

            var viewModel = test.ToDetailsViewModel();
            return View(viewModel);
        }

        [HttpGet("excluir/{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            var test = testRepository.GetRegisterById(id);
            if (test == null)
                return NotFound();

            return View(test.ToDetailsViewModel());
        }

        [HttpPost("excluir/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        { 
            var transaction = dataContext.Database.BeginTransaction();

            try
            {
                testRepository.Delete(id);

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

        [HttpGet("pdf/{id:guid}")]
        public IActionResult GeneratePdf(Guid id, [FromQuery] bool gabarito = false)
        {
            var test = testRepository.GetRegisterById(id);
            if (test == null)
                return NotFound();

            var pdfBytes = pdfGenerator.GenerateTestPdf(test, gabarito);
            return File(pdfBytes, "application/pdf", $"{test.Title}{(gabarito ? "_Gabarito" : "")}.pdf");
        }

        [HttpGet("subjects/{disciplineId:guid}")]
        public IActionResult GetSubjectsByDiscipline(Guid disciplineId)
        {
            var subjects = subjectRepository.GetAllRegisters()
                .Where(s => s.DisciplineId == disciplineId)
                .Select(s => new { s.Id, s.Name })
                .ToList();

            return Json(subjects);
        }
        private IActionResult ReloadViewWithErrors(TestFormViewModel viewModel)
        {
            ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
            ViewBag.GradeLevels = TestFormViewModel.GradeLevels;
            return View(viewModel);
        }

        private IActionResult ReloadDuplicateView(TestDuplicateViewModel viewModel)
        {
            ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
            ViewBag.GradeLevels = TestFormViewModel.GradeLevels;

            if (viewModel.DisciplineId != Guid.Empty)
            {
                var subjects = subjectRepository.GetAllRegisters()
                    .Where(s => s.DisciplineId == viewModel.DisciplineId)
                    .ToList();

                ViewBag.Subjects = subjects;
            }

            return View(viewModel);
        }
    }
}