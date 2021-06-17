using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class Movimiento
    {
        public List<Distar_EntidadesNegocio.Movimiento> obtenerMovimientosCta(string nro_cta_cte)
        {
            Distar_AccesoDatos.Movimiento movimientoDAL = new Distar_AccesoDatos.Movimiento();
            List<Distar_EntidadesNegocio.Movimiento> lista_movimientos = new List<Distar_EntidadesNegocio.Movimiento>();
            lista_movimientos = movimientoDAL.obtenerMovimientosCta(nro_cta_cte);
            return lista_movimientos;
        }

        public Boolean registrarMovimiento(Distar_EntidadesNegocio.Movimiento movimiento)
        {
            Distar_AccesoDatos.Movimiento movimientoDAL = new Distar_AccesoDatos.Movimiento();
            return movimientoDAL.registrarMovimiento(movimiento);
        }
    }
}
