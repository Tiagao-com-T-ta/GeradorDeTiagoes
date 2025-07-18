using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Domain.Shared;
using GeradorDeTiagoes.Structure.Files.Shared;
using GeradorDeTiagoes.WebApp.Extensions;
using GeradorDeTiagoes.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeradorDeTiagoes.WebApp.Controllers
{
    [Route("discipline")]
    public class DisciplineController : Controller
    {
        private readonly DataContext dataContext;
        private readonly IRepository<Discipline> disciplineRepository;

        public DisciplineController(DataContext dataContext, IRepository<Discipline> disciplineRepository)
        {
            this.dataContext = dataContext;
            this.disciplineRepository = disciplineRepository;
        }

        public IActionResult Index()
        {
            var disciplines = disciplineRepository.GetAllRegisters();

            var viewVm = new ViewDisciplineViewModel(disciplines);

            return View(viewVm);
        }

        [HttpGet("register")]
        public IActionResult Create()
        {
            var registerVm = new RegisterDisciplineViewModel();

            return View(registerVm);
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RegisterDisciplineViewModel registerVM)
        {
            var registers = disciplineRepository.GetAllRegisters();

            foreach (var register in registers)
            {
                if (register.Name.Equals(registerVM.Name, StringComparison.OrdinalIgnoreCase))
                {
                    ModelState.AddModelError("Name", "Já existe uma disciplina com este nome.");
                    break;
                }
            }

            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var entity = registerVM.ToEntity();

            disciplineRepository.Register(entity);

            return RedirectToAction(nameof(Index));


        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            var selectedDiscipline = disciplineRepository.GetRegisterById(id);

            var editVm = new EditDisciplineViewModel(
                id,
                selectedDiscipline.Name
                );

            return View(editVm);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, EditDisciplineViewModel editVM)
        {
            var registers = disciplineRepository.GetAllRegisters();

            foreach (var register in registers)
            {
                if (register.Name.Equals(editVM.Name, StringComparison.OrdinalIgnoreCase) && register.Id != id)
                {
                    ModelState.AddModelError("Name", "Já existe uma disciplina com este nome.");
                    break;
                }


            }

            if (!ModelState.IsValid)
            {
                return View(editVM);
            }

            var editedEntity = editVM.ToEntity();

            disciplineRepository.Edit(id, editedEntity);

            return RedirectToAction(nameof(Index));

        }

        [HttpGet("delete/{id:guid}")]
        public ActionResult Delete(Guid id)
        {
            var selectedRegister = disciplineRepository.GetRegisterById(id);

            var deleteVM = new DeleteDisciplineViewModel(
                id,
                selectedRegister.Name
                );

            return View(deleteVM);

        }

        [HttpPost("delete/{id:guid}")]
        public IActionResult ConfirmDelete(Guid id)
        {
            var hasSubjects = dataContext.Subjects.Any(a => a.Discipline.Id == id);
            if (hasSubjects)
            {
                ModelState.AddModelError("", "Não é possível excluir um contato vinculado a um compromisso.");
                var contact = disciplineRepository.GetRegisterById(id);
                var deleteVM = new DeleteDisciplineViewModel(id, contact.Name);
                return View("Delete", deleteVM);
            }

            disciplineRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("details/{id:guid}")]
        public IActionResult Details(Guid id)
        {
            var selectedRegister = disciplineRepository.GetRegisterById(id);

            var detailsVM = new DisciplineDetailsViewModel(
                id,
                selectedRegister.Name
            );

            return View(detailsVM);
        }
    }
}
