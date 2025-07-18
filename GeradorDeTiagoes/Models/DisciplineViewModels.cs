using System.ComponentModel.DataAnnotations;
using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.WebApp.Extensions;

namespace GeradorDeTiagoes.WebApp.Models
{
    public abstract class DisciplineFormViewModel
    {
        [Required(ErrorMessage = "O nome da disciplina é obrigatório.")]
        public string Name { get; set; }
    }

    public class RegisterDisciplineViewModel : DisciplineFormViewModel
    {
        public RegisterDisciplineViewModel() { }
        public RegisterDisciplineViewModel(string name)
        {
            Name = name;
        }
        
    }

    public class EditDisciplineViewModel : DisciplineFormViewModel
    {
        public Guid Id { get; set; }
        public EditDisciplineViewModel() { }
        public EditDisciplineViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class DeleteDisciplineViewModel : DisciplineFormViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public DeleteDisciplineViewModel() { }
        public DeleteDisciplineViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class ViewDisciplineViewModel 
    {
        public List<DisciplineDetailsViewModel> Disciplines { get; set; }
        public ViewDisciplineViewModel(List<Discipline> disciplines)
        {
            Disciplines = new List<DisciplineDetailsViewModel>();

            foreach (var discipline in disciplines)
            {
                Disciplines.Add(discipline.ToDetailsVM());
            }
        }
    }

    public class DisciplineDetailsViewModel 
    { 
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DisciplineDetailsViewModel() { }
        public DisciplineDetailsViewModel(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
      
    }
    
        
    
}
