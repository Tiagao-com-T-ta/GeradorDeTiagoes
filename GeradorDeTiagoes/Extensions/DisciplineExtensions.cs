using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.WebApp.Models;

namespace GeradorDeTiagoes.WebApp.Extensions
{
    public static class DisciplineExtensions
    {
        public static Discipline ToEntity(this DisciplineFormViewModel viewModel)
        {
            return new Discipline(viewModel.Name);
        }

        public static DisciplineDetailsViewModel ToDetailsVM(this Discipline discipline)
        {
            return new DisciplineDetailsViewModel
            (
                discipline.Id,
                discipline.Name
            );
        }
    }
}
