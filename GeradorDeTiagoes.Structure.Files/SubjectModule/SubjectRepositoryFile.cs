using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.SubjectsModule;
using GeradorDeTiagoes.Structure.Files.Shared;
using System;
using System.Collections.Generic;

namespace GeradorDeTiagoes.Structure.Files.SubjectsModule
{
    public class SubjectRepositoryFile : BaseRepositoryFile<Subject>, ISubjectRepository
    {
        public SubjectRepositoryFile(DataContext dataContext) : base(dataContext)
        {
        }

        protected override List<Subject> GetRegisters()
        {
            return dataContext.Subjects;
        }
    }
}
