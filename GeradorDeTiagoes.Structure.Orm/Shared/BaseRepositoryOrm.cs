using GeradorDeTiagoes.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Structure.Orm.Shared
{
    public class BaseRepositoryOrm<T> where T : BaseEntity<T>
    {
        protected readonly GeradorDeTiagoesDbContext dbContext;
        private readonly DbSet<T> registers;

        public BaseRepositoryOrm(GeradorDeTiagoesDbContext context)
        {
            dbContext = context;
            this.registers = context.Set<T>();
        }

        public void Register(T register)
        {
            registers.Add(register);
        }

        public bool Edit(Guid id, T editedRegister)
        {
            var existingRegister = GetRegisterById(id);

            if (existingRegister is null)
                return false;

            existingRegister.Update(editedRegister);
            
            return true;
        }

        public bool Delete(Guid id)
        {
            var existingRegister = GetRegisterById(id);

            if (existingRegister is null)
                return false;

            registers.Remove(existingRegister);
            
            return true;
        }

        public virtual T? GetRegisterById(Guid id)
        {
            return registers.FirstOrDefault(x => x.Id.Equals(id));
        }

        public virtual List<T> GetAllRegisters()
        {
            return registers.ToList();
        }   
    }
}
