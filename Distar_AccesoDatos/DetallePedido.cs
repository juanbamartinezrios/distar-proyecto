using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class DetallePedido
    {
        public List<Distar_EntidadesNegocio.DetallePedido> obtenerDetallePorPedido(int id_pedido)
        {
            List<Distar_EntidadesNegocio.DetallePedido> lista_detalle = new List<Distar_EntidadesNegocio.DetallePedido>();
            string query = "SELECT cantidad, id_detalle_pedido, DetallePedido.id_producto, importe, nro_detalle, descripcion, p_unitario FROM DetallePedido INNER JOIN Producto ON DetallePedido.id_producto = Producto.id_producto WHERE id_pedido=" + id_pedido + " ORDER BY nro_detalle ASC";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.DetallePedido item = new Distar_EntidadesNegocio.DetallePedido();
                    item.cantidad = Convert.ToInt32(dataReader["cantidad"]);
                    item.id_detalle_pedido = Convert.ToInt32(dataReader["id_detalle_pedido"]);
                    item.id_pedido = id_pedido;
                    item.id_producto = Convert.ToInt32(dataReader["id_producto"]);
                    item.importe = float.Parse(Convert.ToString(dataReader["importe"]));
                    item.nro_detalle = Convert.ToInt32(dataReader["nro_detalle"]);
                    item.descripcion_prod = Convert.ToString(dataReader["descripcion"]);
                    item.p_unitario_prod = float.Parse(Convert.ToString(dataReader["p_unitario"]));
                    lista_detalle.Add(item);
                }
                dataReader.Close();
                return lista_detalle;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL DetallePedido obtenerDetallePorPedido: " + ex.Message);
                return lista_detalle;
            }
        }

        public Boolean agregarDetalleAPedido(List<Distar_EntidadesNegocio.DetallePedido> lista_add)
        {
            Boolean flag = true;
            foreach (Distar_EntidadesNegocio.DetallePedido item in lista_add)
            {
                string query = "INSERT INTO DetallePedido VALUES (" + item.cantidad + ", " + item.id_pedido + "," + item.id_producto + ", CONVERT(float, '" + Convert.ToString(item.importe).Replace(",", ".") + "'), " + item.nro_detalle + ")";
                if (Distar_BD.ExecuteNonquery(query) == -1)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        public Boolean eliminarDetallesDePedido(int id_pedido)
        {
            string query = "DELETE FROM DetallePedido WHERE id_pedido=" + id_pedido;
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL DetallePedido eliminarDetallesDePedido: " + ex.Message);
                return false;
            }
        }
    }
}
