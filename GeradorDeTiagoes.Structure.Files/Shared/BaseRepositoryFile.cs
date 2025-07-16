using GeradorDeTiagoes.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Structure.Files.Shared
{
    public abstract class BaseRepositoryFile<T> where T : BaseEntity<T>
    {
        protected DataContext dataContext;
        protected List<T> registers;

        protected BaseRepositoryFile(DataContext dataContext)
        {
            this.dataContext = dataContext;
            registers = GetRegisters();
        }

        protected abstract List<T> GetRegisters();

        public void Register(T newRegister)
        {
            registers.Add(newRegister);
            dataContext.SaveData();
        }

        public bool Edit(Guid registerId, T editedRegister)
        {
            var register = GetRegisterById(registerId);

            if (register == null)
                return false;

            register.Update(editedRegister);
            dataContext.SaveData();
            return true;
        }

        public bool Delete(Guid registerId)
        {
            var register = GetRegisterById(registerId);

            if (register == null)
                return false;

            registers.Remove(register);
            dataContext.SaveData();
            return true;
        }

        public List<T> GetAllRegisters()
        {
            return registers;
        }

        public T GetRegisterById(Guid registerId)
        {
            return registers.FirstOrDefault(r => r.Id == registerId)!;
        }
    }
}
