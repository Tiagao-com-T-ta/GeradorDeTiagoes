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

namespace GeradorDeTiagoes.WebApp.Controllers
{
    [Route("tests")]
    public class TestController : Controller
    {
        private readonly IRepository<Test> testRepository;
        private readonly IRepository<Discipline> disciplineRepository;
        private readonly IRepository<Subject> subjectRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IPdfGenerator pdfGenerator;

        public TestController(
            IRepository<Test> testRepository,
            IRepository<Discipline> disciplineRepository,
            IRepository<Subject> subjectRepository,
            IQuestionRepository questionRepository,
            IPdfGenerator pdfGenerator)
        {
            this.testRepository = testRepository;
            this.disciplineRepository = disciplineRepository;
            this.subjectRepository = subjectRepository;
            this.questionRepository = questionRepository;
            this.pdfGenerator = pdfGenerator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var tests = testRepository.GetAllRegisters()
                .Select(t => new Test
                {
                    Id = t.Id,
                    Title = t.Title,
                    GradeLevel = t.GradeLevel,
                    DisciplineId = t.DisciplineId,
                    SubjectId = t.SubjectId,
                    QuestionCount = t.QuestionCount,
                    IsRecovery = t.IsRecovery,
                    Discipline = disciplineRepository.GetRegisterById(t.DisciplineId),
                    Subject = t.SubjectId.HasValue ?
                        subjectRepository.GetRegisterById(t.SubjectId.Value) : null,
                    Questions = t.Questions
                }).ToList();

            var viewModel = new TestListViewModel(tests);
            return View(viewModel);
        }

        [HttpGet("gerar")]
        public IActionResult Create()
        {
            ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
            ViewBag.GradeLevels = GetGradeLevels();
            return View(new TestFormViewModel());
        }

        [HttpPost("gerar")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TestFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
                ViewBag.GradeLevels = GetGradeLevels();
                return View(viewModel);
            }

            if (testRepository.GetAllRegisters()
                .Any(t => t.Title.Equals(viewModel.Title, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Title", "Já existe um teste com este título.");
                ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
                ViewBag.GradeLevels = GetGradeLevels();
                return View(viewModel);
            }

            if (!viewModel.IsRecovery && viewModel.SubjectId.HasValue)
            {
                var subject = subjectRepository.GetRegisterById(viewModel.SubjectId.Value);
                if (subject == null || subject.DisciplineId != viewModel.DisciplineId)
                {
                    ModelState.AddModelError("SubjectId", "Matéria não pertence à disciplina selecionada.");
                    ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
                    ViewBag.GradeLevels = GetGradeLevels();
                    return View(viewModel);
                }
            }

            // Obter questões disponíveis - erro quando não tem questão, adicionar validação
            var availableQuestions = viewModel.IsRecovery
                ? questionRepository.GetAllByDiscipline(viewModel.DisciplineId)
                : questionRepository.GetAllBySubject(viewModel.SubjectId.Value);

            if (availableQuestions.Count < viewModel.QuestionCount)
            {
                ModelState.AddModelError("QuestionCount",
                    $"Quantidade insuficiente de questões disponíveis ({availableQuestions.Count}).");
                ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
                ViewBag.GradeLevels = GetGradeLevels();
                return View(viewModel);
            }

            // Criar teste com questões aleatórias - ainda não testado
            var test = viewModel.ToEntity();
            var random = new Random();
            test.Questions = availableQuestions
                .OrderBy(q => random.Next())
                .Take(viewModel.QuestionCount)
                .ToList();

            testRepository.Register(test);

            return RedirectToAction(nameof(Details), new { id = test.Id });
        }

        [HttpGet("duplicar/{id:guid}")]
        public IActionResult Duplicate(Guid id)
        {
            var originalTest = testRepository.GetRegisterById(id);
            if (originalTest == null)
                return NotFound();

            var viewModel = originalTest.ToDuplicateViewModel();
            ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
            ViewBag.GradeLevels = GetGradeLevels();
            return View(viewModel);
        }

        [HttpPost("duplicar/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult Duplicate(Guid id, TestDuplicateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
                ViewBag.GradeLevels = GetGradeLevels();
                return View(viewModel);
            }

            if (testRepository.GetAllRegisters().Any(t => t.Title.Equals(viewModel.Title, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Title", "Já existe um teste com este título.");
                ViewBag.Disciplines = disciplineRepository.GetAllRegisters();
                ViewBag.GradeLevels = GetGradeLevels();
                return View(viewModel);
            }

            var newTest = viewModel.ToEntity();
            testRepository.Register(newTest);

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
            testRepository.Delete(id);
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

        private List<string> GetGradeLevels()
        {
            return new List<string>
            {
                "1º Ano", "2º Ano", "3º Ano", "4º Ano", "5º Ano",
                "6º Ano", "7º Ano", "8º Ano", "9º Ano",
                "1ª Série EM", "2ª Série EM", "3ª Série EM"
            };
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
    }
}