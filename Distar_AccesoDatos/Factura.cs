using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace Distar_AccesoDatos
{
    public class Factura
    {
        public List<Distar_EntidadesNegocio.Factura> getAllFacturas()
        {
            List<Distar_EntidadesNegocio.Factura> listaFacturas = new List<Distar_EntidadesNegocio.Factura>();
            string query = "SELECT * FROM Factura ORDER BY fecha_emision DESC";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Factura factura = new Distar_EntidadesNegocio.Factura();
                    factura.tipo_factura = Convert.ToString(dataReader["tipo_factura"]);
                    factura.iva = Convert.ToInt32(dataReader["iva"]);
                    factura.fecha_emision = Convert.ToDateTime(dataReader["fecha_emision"]);
                    factura.id_cliente = Convert.ToInt32(dataReader["id_cliente"]);
                    factura.id_factura = Convert.ToInt32(dataReader["id_factura"]);
                    factura.nro_factura = Convert.ToInt32(dataReader["nro_factura"]);
                    listaFacturas.Add(factura);
                }
                dataReader.Close();
                return listaFacturas;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Factura getAllFacturas: " + ex.Message);
                return listaFacturas;
            }
        }

        public Boolean create(Distar_EntidadesNegocio.Factura factura)
        {
            string query = "INSERT INTO Factura VALUES (CONVERT(datetime, '" + factura.fecha_emision.ToString("yyyy-MM-dd HH:mm") + "', 101), " + factura.id_cliente + ", " + factura.iva + ", " + factura.nro_factura + ", '" + factura.tipo_factura + "')";
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Factura create: " + ex.Message);
                return false;
            }
        }

        public int getIdFactura(int nro_factura)
        {
            string query = "SELECT id_factura FROM Factura WHERE nro_factura=" + nro_factura;
            Distar_EntidadesNegocio.Factura factura = new Distar_EntidadesNegocio.Factura();
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    factura.id_factura = Convert.ToInt32(dataReader["id_factura"]);
                }
                dataReader.Close();
                return factura.id_factura;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Factura getIdFactura: " + ex.Message);
                return 0;
            }
        }
    }
}
