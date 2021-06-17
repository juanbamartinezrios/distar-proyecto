using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public class Impresion
    {
        public int id_impresion { get; set; }
        public DateTime fecha { get; set; }
        public int id_usuario { get; set; }
        public string reporte { get; set; }
    }
}
