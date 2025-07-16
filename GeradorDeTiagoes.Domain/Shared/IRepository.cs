using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Domain.Shared
{
    public interface IRepository<T> where T : BaseEntity<T>
    {
        public void Register(T newRegister);

        public bool Edit(Guid registerId, T editedRegister);

        public bool Delete(Guid registerId);

        public List<T> GetAllRegisters();

        public T GetRegisterById(Guid registerId);
    }

}
