using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public interface ABMGenerico<T>
    {
        Boolean create(T entidad);
        Boolean delete(T entidad);
        Boolean deleteLogico(T entidad);
        Boolean update(T entidad);
    }
}
