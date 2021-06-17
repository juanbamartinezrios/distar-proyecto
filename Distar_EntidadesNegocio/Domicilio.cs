using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public class Domicilio
    {
        public string cp { get; set; }
        public string direccion { get; set; }
        public int id_domicilio { get; set; }
        public int id_usuario { get; set; }
        public string numero_dom { get; set; }
    }
}
