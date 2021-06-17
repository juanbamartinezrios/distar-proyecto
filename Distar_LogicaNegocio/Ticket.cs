using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_LogicaNegocio
{
    public class Ticket
    {
        public int calcularDVHPorRegistro(string id_ticket)
        {
            Distar_AccesoDatos.Ticket ticketDAL = new Distar_AccesoDatos.Ticket();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            string str = ticketDAL.obtenerStrParaDVHPorTicket(id_ticket);
            return DVBL.calcularDVH(str);
        }

        public Boolean pagarDeuda(int id_ticket, Distar_EntidadesNegocio.Movimiento movimiento)
        {
            Distar_LogicaNegocio.Movimiento movimientoBL = new Distar_LogicaNegocio.Movimiento();
            Distar_AccesoDatos.Ticket ticketDAL = new Distar_AccesoDatos.Ticket();
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            if (ticketDAL.pagarDeuda(id_ticket))
            {
                DVBL.actualizarDVHRegistros("Ticket", id_ticket);
                movimientoBL.registrarMovimiento(movimiento);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean create(Distar_EntidadesNegocio.Ticket ticket)
        {
            Distar_LogicaNegocio.CuentaCorriente cuentaCorrienteBL = new Distar_LogicaNegocio.CuentaCorriente();
            Distar_AccesoDatos.Ticket ticketDAL = new Distar_AccesoDatos.Ticket();
            ticket.nro_ticket = getRandomNroTicket(ticket.nro_factura.ToString());
            Distar_LogicaNegocio.DigitoVerificador DVBL = new Distar_LogicaNegocio.DigitoVerificador();
            ticket.DVH = DVBL.calcularDVH(ticket.estado + ticket.nro_factura.ToString() + ticket.nro_ticket.ToString() + ticket.id_cliente.ToString());
            if (ticketDAL.create(ticket))
            {
                int id_ticket = this.getIdTicket(ticket.nro_ticket);
                DVBL.actualizarDVHRegistros("Ticket", id_ticket);
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getRandomNroTicket(string nro_factura)
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
            randomStr = substr1 + substr2 + nro_factura.Substring(0, 4);
            return Convert.ToInt32(randomStr);
        }

        public List<Distar_EntidadesNegocio.Ticket> getAllTickets(int id_cliente)
        {
            Distar_LogicaNegocio.DetalleFactura detalleFacturaBL = new Distar_LogicaNegocio.DetalleFactura();
            Distar_LogicaNegocio.Factura facturaBL = new Distar_LogicaNegocio.Factura();
            Distar_LogicaNegocio.Usuario usuarioBL = new Distar_LogicaNegocio.Usuario();
            Distar_AccesoDatos.Ticket ticketDAL = new Distar_AccesoDatos.Ticket();
            Distar_AccesoDatos.Services services = new Distar_AccesoDatos.Services();
            List<Distar_EntidadesNegocio.Ticket> lista_tickets = new List<Distar_EntidadesNegocio.Ticket>();
            foreach (Distar_EntidadesNegocio.Ticket ticket in ticketDAL.getAllTickets(id_cliente))
            {
                ticket.cliente = usuarioBL.obtenerUsuarioPorId(ticket.id_cliente);
                if (ticket.cliente != null)
                {
                    ticket.cliente.domicilio.cp = services.Desencriptar3D(ticket.cliente.domicilio.cp);
                    ticket.cliente.domicilio.direccion = services.Desencriptar3D(ticket.cliente.domicilio.direccion);
                }
                ticket.detalle_items = detalleFacturaBL.cargarDetalleDeFactura(facturaBL.getIdFactura(ticket.nro_factura));
                foreach (Distar_EntidadesNegocio.DetalleFactura item in ticket.detalle_items)
                {
                    ticket.total += item.importe;
                }
                lista_tickets.Add(ticket);
            }
            return lista_tickets;
        }

        public int getIdTicket(int nro_ticket)
        {
            Distar_AccesoDatos.Ticket ticketDAL = new Distar_AccesoDatos.Ticket();
            return ticketDAL.getIdTicket(nro_ticket);
        }
    }
}
