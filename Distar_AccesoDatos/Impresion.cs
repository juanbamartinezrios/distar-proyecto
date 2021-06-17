using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar_AccesoDatos
{
    public class Impresion
    {
        public Boolean registrarImpresion(Distar_EntidadesNegocio.Impresion nueva_impresion)
        {
            string query = "INSERT INTO Impresion VALUES (CONVERT(datetime, '" + nueva_impresion.fecha.ToString("yyyy-MM-dd HH:mm") + "', 101), " + nueva_impresion.id_usuario + ", '" + nueva_impresion.reporte + "')";
            try
            {
                return Distar_BD.ExecuteNonquery(query) > -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DAL Impresion registrarImpresion: " + ex.Message);
                return false;
            }
        }
    }
}
