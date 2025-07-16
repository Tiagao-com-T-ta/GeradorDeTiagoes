using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeTiagoes.Domain.Shared
{
    public abstract class BaseEntity<T>
    {
        public Guid Id { get; set; }

        public abstract void Update(T EditedEntity);
    }
}
