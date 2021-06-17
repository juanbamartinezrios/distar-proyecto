using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class Factura
    {
        public List<Distar_EntidadesNegocio.Factura> getAllFacturas()
        {
            Distar_LogicaNegocio.DetalleFactura detalleFacturaBL = new Distar_LogicaNegocio.DetalleFactura();
            Distar_LogicaNegocio.Usuario usuarioBL = new Distar_LogicaNegocio.Usuario();
            Distar_AccesoDatos.Factura facturaDAL = new Distar_AccesoDatos.Factura();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            List<Distar_EntidadesNegocio.Factura> lista_facturas = new List<Distar_EntidadesNegocio.Factura>();
            foreach (Distar_EntidadesNegocio.Factura factura in facturaDAL.getAllFacturas())
            {
                factura.cliente = usuarioBL.obtenerUsuarioPorId(factura.id_cliente);
                //if (factura.cliente != null)
                //{
                //    factura.cliente.domicilio.cp = services.Desencriptar3D(factura.cliente.domicilio.cp);
                //    factura.cliente.domicilio.direccion = services.Desencriptar3D(factura.cliente.domicilio.direccion);
                //}
                factura.detalle_items = detalleFacturaBL.cargarDetalleDeFactura(factura.id_factura);
                foreach (Distar_EntidadesNegocio.DetalleFactura item in factura.detalle_items)
                {
                    factura.total += item.importe;
                }
                lista_facturas.Add(factura);
            }
            return lista_facturas;
        }

        public Boolean create(Distar_EntidadesNegocio.Factura factura, Distar_EntidadesNegocio.Ticket ticket)
        {
            Distar_LogicaNegocio.DetalleFactura detalleFactura = new Distar_LogicaNegocio.DetalleFactura();
            Distar_LogicaNegocio.Ticket ticketBL = new Distar_LogicaNegocio.Ticket();
            Distar_LogicaNegocio.Factura facturaBL = new Distar_LogicaNegocio.Factura();
            Distar_AccesoDatos.Factura facturaDAL = new Distar_AccesoDatos.Factura();
            factura.nro_factura = getRandomNroFactura(factura.id_cliente.ToString());
            if (facturaDAL.create(factura))
            {
                int nro_detalle = 0;
                int id_factura = facturaBL.getIdFactura(factura.nro_factura);
                foreach (Distar_EntidadesNegocio.DetalleFactura item in factura.detalle_items)
                {
                    item.nro_detalle = nro_detalle++;
                    item.id_factura = id_factura;
                }
                detalleFactura.agregarDetalleAFactura(factura.detalle_items);
                ticket.nro_factura = factura.nro_factura;
                try
                {
                    if (ticketBL.create(ticket))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public int getRandomNroFactura(string id_cliente)
        {
            string randomStr;
            string substr1;
            string substr2;
            string s = "123456789";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                int idx = rd.Next(0, 5);
                sb.Append(s.Substring(idx, 1));
            }
            randomStr = sb.ToString();
            substr1 = randomStr.Substring(0, 2);
            substr2 = randomStr.Substring(7, 2);
            randomStr = substr1 + substr2 + id_cliente;
            return Convert.ToInt32(randomStr);
        }

        public int getIdFactura(int nro_factura)
        {
            Distar_AccesoDatos.Factura facturaDAL = new Distar_AccesoDatos.Factura();
            return facturaDAL.getIdFactura(nro_factura);
        }
    }
}
