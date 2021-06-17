using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Distar_AccesoDatos
{
    public class Movimiento
    {
        public List<Distar_EntidadesNegocio.Movimiento> obtenerMovimientosCta(string nro_cta_cte)
        {
            List<Distar_EntidadesNegocio.Movimiento> lista_movimientos = new List<Distar_EntidadesNegocio.Movimiento>();
            string query = "SELECT * FROM Movimiento WHERE nro_cta='" + nro_cta_cte+"'";
            SqlDataReader dataReader;
            try
            {
                dataReader = Distar_BD.ExecuteReader(query);
                while (dataReader.Read())
                {
                    Distar_EntidadesNegocio.Movimiento item = new Distar_EntidadesNegocio.Movimiento();
                    item.fecha_mov = Convert.ToDateTime(dataReader["fecha_mov"]);
                    item.id_movimiento = Convert.ToInt32(dataReader["id_movimiento"]);
                    item.monto = float.Parse(Convert.ToString(dataReader["monto"]));
                    item.nro_cta = Convert.ToString(dataReader["nro_cta"]);
                    item.tipo_mov = Convert.ToString(dataReader["tipo_mov"]);
                    lista_movimientos.Add(item);
                }
                dataReader.Close();
                return lista_movimientos;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Movimiento obtenerMovimientosCta: " + ex.Message);
                return lista_movimientos;
            }
        }

        public Boolean registrarMovimiento(Distar_EntidadesNegocio.Movimiento movimiento)
        {
            string query = "INSERT INTO Movimiento VALUES (CONVERT(datetime, '" + movimiento.fecha_mov.ToString("yyyy-MM-dd HH:mm") + "', 101), CONVERT(float, '" + Convert.ToString(movimiento.monto).Replace(",", ".") + "'), '" + movimiento.nro_cta + "', '" + movimiento.tipo_mov + "')";
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Movimiento registrarMovimiento: " + ex.Message);
                return false;
            }
        }
    }
}
