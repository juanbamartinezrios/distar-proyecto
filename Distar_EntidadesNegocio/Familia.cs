using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public class Familia
    {
        public int id_familia { get; set; }
        public string descripcion { get; set; }
        public List<Patente> patentes { get; set; }
    }
}
