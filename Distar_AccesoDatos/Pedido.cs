using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class Pedido
    {
        public List<Distar_EntidadesNegocio.Pedido> getAllPedidos()
        {
            List<Distar_EntidadesNegocio.Pedido> listaPedidos = new List<Distar_EntidadesNegocio.Pedido>();
            string query = "SELECT descripcion, estado, fecha_creacion, id_cliente, id_pedido, nro_pedido FROM Pedido ORDER BY fecha_creacion DESC";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Pedido pedido = new Distar_EntidadesNegocio.Pedido();
                    pedido.descripcion = Convert.ToString(dataReader["descripcion"]);
                    pedido.estado = Convert.ToString(dataReader["estado"]);
                    pedido.fecha_creacion = Convert.ToDateTime(dataReader["fecha_creacion"]);
                    pedido.id_cliente = Convert.ToInt32(dataReader["id_cliente"]);
                    pedido.id_pedido = Convert.ToInt32(dataReader["id_pedido"]);
                    pedido.nro_pedido = Convert.ToInt32(dataReader["nro_pedido"]);
                    listaPedidos.Add(pedido);
                }
                dataReader.Close();
                return listaPedidos;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Pedido getAllPedidos: " + ex.Message);
                return listaPedidos;
            }
        }

        public Boolean create(Distar_EntidadesNegocio.Pedido pedido){
            string query = "INSERT INTO Pedido VALUES ('" + pedido.descripcion + "', '" + pedido.estado + "', CONVERT(datetime, '" + pedido.fecha_creacion.ToString("yyyy-MM-dd HH:mm") + "', 101), " + pedido.id_cliente + ", " + pedido.nro_pedido+ ")";
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Pedido create: " + ex.Message);
                return false;
            }
        }

        public Boolean delete(int id_pedido)
        {
            string query = "DELETE FROM Pedido WHERE id_pedido=" + id_pedido;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Pedido delete: " + ex.Message);
                return false;
            }
        }

        public int getIdPedido(int nro_pedido)
        {
            string query = "SELECT id_pedido FROM Pedido WHERE nro_pedido=" + nro_pedido;
            Distar_EntidadesNegocio.Pedido pedido = new Distar_EntidadesNegocio.Pedido();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    pedido.id_pedido = Convert.ToInt32(dataReader["id_pedido"]);
                }
                dataReader.Close();
                return pedido.id_pedido;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Pedido getIdPedido: " + ex.Message);
                return 0;
            }
        }

        public Boolean rechazarPedido(string descripcion, int id_pedido)
        {
            string query = "UPDATE Pedido SET estado='Rechazado', descripcion='" + descripcion + "' WHERE id_pedido=" + id_pedido;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Pedido rechazarPedido: " + ex.Message);
                return false;
            }
        }
    }
}
