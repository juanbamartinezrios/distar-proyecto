using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public class Movimiento
    {
        public DateTime fecha_mov { get; set; }
        public int id_movimiento { get; set; }
        public float monto { get; set; }
        public string nro_cta { get; set; }
        public string tipo_mov { get; set; }
    }
}
