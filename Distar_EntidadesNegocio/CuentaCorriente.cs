using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_EntidadesNegocio
{
    public class CuentaCorriente
    {
        public int id_cliente { get; set; }
        public int id_cta_cte { get; set; }
        public string nro_cta { get; set; }
        public float saldo_deudor { get; set; }
        public int DVH { get; set; }
        public List<Movimiento> movimientos { get; set; }
    }
}
