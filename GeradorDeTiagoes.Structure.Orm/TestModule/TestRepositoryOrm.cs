using GeradorDeTiagoes.Domain.Entities;
using GeradorDeTiagoes.Domain.TestsModule;
using GeradorDeTiagoes.Structure.Orm.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Structure.Orm.TestModule
{
    public class TestRepositoryOrm : BaseRepositoryOrm<Test>, ITestRepository
    {
        public TestRepositoryOrm(GeradorDeTiagoesDbContext context) : base(context) { }

        public override Test? GetRegisterById(Guid id)
        {
            return dbContext.Tests
                .Include(t => t.Discipline)
                .Include(t => t.Subject)
                .Include(t => t.Questions)
                .ThenInclude(q => q.Alternatives)
                .FirstOrDefault(t => t.Id == id);
        }

        public override List<Test> GetAllRegisters()
        {
            return dbContext.Tests
                .Include(t => t.Discipline)
                .Include(t => t.Subject)
                .Include(t => ((Test)t).Questions)
                .ThenInclude(q => q.Alternatives)
                .ToList();
        }

    }
}
