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
        private readonly DbSet<T> registers;

        public BaseRepositoryOrm(GeradorDeTiagoesDbContext context)
        {
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

            registers.Update(existingRegister);
            
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
