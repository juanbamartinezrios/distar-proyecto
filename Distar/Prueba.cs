using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distar
{
    class Prueba
    {
        
        public void hacerPruebaConexionBD()
        {
            Console.WriteLine("Conexión BD:");
            Distar_AccesoDatos.Distar_BD.ProbarConexion(Distar_AccesoDatos.Distar_BD.connectionString);
        }
    }
}
