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
            var tests = dataContext.Tests;

            foreach (var test in tests)
            {
                test.Discipline = dataContext.Disciplines.FirstOrDefault(d => d.Id == test.DisciplineId);
                test.Subject = test.SubjectId.HasValue
                    ? dataContext.Subjects.FirstOrDefault(s => s.Id == test.SubjectId.Value)
                    : null;
            }

            return tests;
        }
    }
}
