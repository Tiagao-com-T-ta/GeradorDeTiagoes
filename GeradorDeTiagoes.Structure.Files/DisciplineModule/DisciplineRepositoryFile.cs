using GeradorDeTiagoes.Domain.DisciplineModule;
using GeradorDeTiagoes.Structure.Files.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Structure.Files.DisciplineModule
{
    public class DisciplineRepositoryFile : BaseRepositoryFile<Discipline>, IDisciplineRepository
    {
        public DisciplineRepositoryFile(DataContext dataContext) : base(dataContext) { }

        protected override List<Discipline> GetRegisters()
        {
            return dataContext.Disciplines;
        }
    }
}
