using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.TestsModule;
using GeradorDeTiagoes.Structure.Files.Shared;
using System;
using System.Collections.Generic;

namespace GeradorDeTiagoes.Structure.Files.TestsModule
{
    public class TestRepositoryFile : BaseRepositoryFile<Test>, ITestRepository
    {
        public TestRepositoryFile(DataContext dataContext) : base(dataContext)
        {
        }

        protected override List<Test> GetRegisters()
        {
            return dataContext.Tests;
        }
    }
}
