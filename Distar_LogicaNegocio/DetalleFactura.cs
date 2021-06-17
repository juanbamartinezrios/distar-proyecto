using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class DetalleFactura
    {
        public List<Distar_EntidadesNegocio.DetalleFactura> cargarDetalleDeFactura(int id_factura)
        {
            Distar_AccesoDatos.DetalleFactura detalleFacturaDAL = new Distar_AccesoDatos.DetalleFactura();
            List<Distar_EntidadesNegocio.DetalleFactura> lista_detalles = new List<Distar_EntidadesNegocio.DetalleFactura>();
            lista_detalles = detalleFacturaDAL.obtenerDetallePorFactura(id_factura);
            return lista_detalles;
        }

        public Boolean agregarDetalleAFactura(List<Distar_EntidadesNegocio.DetalleFactura> lista_detalles_add)
        {
            Distar_AccesoDatos.DetalleFactura detalleFacturaDAL = new Distar_AccesoDatos.DetalleFactura();
            if (detalleFacturaDAL.agregarDetalleAFactura(lista_detalles_add))
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
