using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace Distar_AccesoDatos
{
    public class DetalleFactura
    {
        public List<Distar_EntidadesNegocio.DetalleFactura> obtenerDetallePorFactura(int id_factura)
        {
            List<Distar_EntidadesNegocio.DetalleFactura> lista_detalle = new List<Distar_EntidadesNegocio.DetalleFactura>();
            string query = "SELECT cantidad, id_detalle_factura, DetalleFactura.id_producto, importe, nro_detalle, descripcion, p_unitario FROM DetalleFactura INNER JOIN Producto ON DetalleFactura.id_producto = Producto.id_producto WHERE id_factura=" + id_factura + " ORDER BY nro_detalle ASC";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.DetalleFactura item = new Distar_EntidadesNegocio.DetalleFactura();
                    item.cantidad = Convert.ToInt32(dataReader["cantidad"]);
                    item.id_detalle_factura = Convert.ToInt32(dataReader["id_detalle_factura"]);
                    item.id_factura = id_factura;
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
                Console.WriteLine("DAL DetalleFactura obtenerDetallePorFactura: " + ex.Message);
                return lista_detalle;
            }
        }

        public Boolean agregarDetalleAFactura(List<Distar_EntidadesNegocio.DetalleFactura> lista_add)
        {
            Boolean flag = true;
            foreach (Distar_EntidadesNegocio.DetalleFactura item in lista_add)
            {
                string query = "INSERT INTO DetalleFactura VALUES (" + item.cantidad + ", " + item.id_factura + "," + item.id_producto + ", CONVERT(float, '" + Convert.ToString(item.importe).Replace(",", ".") + "'), " + item.nro_detalle + ")";
                if (Distar_BD.ExecuteNonquery(query) == -1)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
    }
}
