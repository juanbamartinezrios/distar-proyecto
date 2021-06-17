using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public class Telefono
    {
        public int id_telefono { get; set; }
        public int id_usuario { get; set; }
        public string telefono { get; set; }
        public string telefono_alt { get; set; }
    }
}
