using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class Ticket
    {
        public string obtenerStrParaDVHPorTicket(string id_ticket)
        {
            string query = "SELECT estado, nro_factura, nro_ticket, id_cliente FROM Ticket WHERE id_ticket=" + id_ticket;
            string str = null;
            SqlDataReader dataReader;
            dataReader = Distar_BD.ExecuteReader(query);
            if (dataReader != null && dataReader.HasRows)
            {
                dataReader.Read();
                str = dataReader["estado"].ToString() + dataReader["nro_factura"].ToString() + dataReader["nro_ticket"].ToString() + dataReader["id_cliente"].ToString();
                dataReader.Close();
            }
            return str;
        }

        public Boolean actualizarDVH(string valorActualizado, int id_registro)
        {
            string query = "UPDATE Ticket SET DVH=" + valorActualizado + " WHERE id_ticket=" + id_registro.ToString();
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Ticket actualizarDVH: " + ex.Message);
                return false;
            }
        }

        public Boolean pagarDeuda(int id_ticket)
        {
            string query = "UPDATE Ticket SET estado='Pago' WHERE id_ticket="+id_ticket;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Ticket pagarDeuda: " + ex.Message);
                return false;
            }
        }

        public Boolean create(Distar_EntidadesNegocio.Ticket ticket)
        {
            string query = "INSERT INTO Ticket VALUES (" + ticket.DVH + ", '" + ticket.estado + "', CONVERT(datetime, '" + ticket.fecha_emision.ToString("yyyy-MM-dd HH:mm") + "', 101), " + ticket.id_cliente + ", " + ticket.nro_factura + ", " + ticket.nro_ticket + ")";
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Ticket create: " + ex.Message);
                return false;
            }
        }

        public List<Distar_EntidadesNegocio.Ticket> getAllTickets(int id_cliente)
        {
            List<Distar_EntidadesNegocio.Ticket> listaTickets = new List<Distar_EntidadesNegocio.Ticket>();
            string query = "SELECT * FROM Ticket WHERE id_cliente="+id_cliente+" ORDER BY fecha_emision DESC";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Ticket ticket = new Distar_EntidadesNegocio.Ticket();
                    ticket.estado = Convert.ToString(dataReader["estado"]);
                    ticket.fecha_emision = Convert.ToDateTime(dataReader["fecha_emision"]);
                    ticket.DVH = Convert.ToInt32(dataReader["DVH"]);
                    ticket.id_cliente = Convert.ToInt32(dataReader["id_cliente"]);
                    ticket.id_ticket = Convert.ToInt32(dataReader["id_ticket"]);
                    ticket.nro_ticket = Convert.ToInt32(dataReader["nro_ticket"]);
                    ticket.nro_factura = Convert.ToInt32(dataReader["nro_factura"]);
                    listaTickets.Add(ticket);
                }
                dataReader.Close();
                return listaTickets;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Ticket getAllTickets: " + ex.Message);
                return listaTickets;
            }
        }

        public int getIdTicket(int nro_ticket)
        {
            string query = "SELECT id_ticket FROM Ticket WHERE nro_ticket=" + nro_ticket;
            Distar_EntidadesNegocio.Ticket ticket = new Distar_EntidadesNegocio.Ticket();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    ticket.id_ticket = Convert.ToInt32(dataReader["id_ticket"]);
                }
                dataReader.Close();
                return ticket.id_ticket;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Ticket getIdTicket: " + ex.Message);
                return 0;
            }
        }

        public List<Distar_EntidadesNegocio.DetalleFactura> getImportes(int id_cliente)
        {
            List<Distar_EntidadesNegocio.DetalleFactura> lista_detalles = new List<Distar_EntidadesNegocio.DetalleFactura>();
            string query = "SELECT importe FROM DetalleFactura INNER JOIN (Ticket INNER JOIN Factura ON Ticket.nro_factura=Factura.nro_factura AND Ticket.estado='Impago') ON DetalleFactura.id_factura=Factura.id_factura WHERE Ticket.id_cliente=" + id_cliente;
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.DetalleFactura detalle = new Distar_EntidadesNegocio.DetalleFactura();
                    detalle.importe = float.Parse(Convert.ToString(dataReader["importe"]));
                    lista_detalles.Add(detalle);
                }
                dataReader.Close();
                return lista_detalles;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Ticket getImportes: " + ex.Message);
                return lista_detalles;
            }
        }

        public List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> obtenerDTO_DV()
        {
            string query = "SELECT id_ticket, estado, nro_ticket, nro_factura, id_cliente, DVH FROM Ticket";
            List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO> lista_dv_dto = new List<Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO>();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO dto = new Distar_EntidadesNegocio.DTO.DigitoVerificadorDTO();
                    dto.entidad = "Ticket";
                    dto.valor_db = Convert.ToInt32(dataReader["DVH"]);
                    dto.id_registro = Convert.ToInt32(dataReader["id_ticket"]);
                    dto.txtstr = Convert.ToString(dataReader["estado"]) + Convert.ToInt32(dataReader["nro_factura"]).ToString() + Convert.ToInt32(dataReader["nro_ticket"]).ToString() + Convert.ToInt32(dataReader["id_cliente"]).ToString();
                    lista_dv_dto.Add(dto);
                }
                dataReader.Close();
                return lista_dv_dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Ticket obtenerDTO_DV: " + ex.Message);
                return lista_dv_dto;
            }
        }
    }
}
