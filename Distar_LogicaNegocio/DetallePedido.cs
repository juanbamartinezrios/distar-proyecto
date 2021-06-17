using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class DetallePedido
    {
        public List<Distar_EntidadesNegocio.DetallePedido> cargarDetalleDePedido(int id_pedido)
        {
            Distar_AccesoDatos.DetallePedido detallePedidoDAL = new Distar_AccesoDatos.DetallePedido();
            List<Distar_EntidadesNegocio.DetallePedido> lista_detalles = new List<Distar_EntidadesNegocio.DetallePedido>();
            lista_detalles = detallePedidoDAL.obtenerDetallePorPedido(id_pedido);
            return lista_detalles;
        }

        public Boolean agregarDetalleAPedido(List<Distar_EntidadesNegocio.DetallePedido> lista_detalles_add)
        {
            Distar_AccesoDatos.DetallePedido detallePedidoDAL = new Distar_AccesoDatos.DetallePedido();
            if (detallePedidoDAL.agregarDetalleAPedido(lista_detalles_add))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean eliminarDetallesDePedido(int id_pedido)
        {
            Distar_AccesoDatos.DetallePedido detallePedidoDAL = new Distar_AccesoDatos.DetallePedido();
            if (detallePedidoDAL.eliminarDetallesDePedido(id_pedido))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
